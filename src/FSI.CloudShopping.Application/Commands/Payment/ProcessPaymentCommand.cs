namespace FSI.CloudShopping.Application.Commands.Payment;

using MediatR;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Enums;
using FSI.CloudShopping.Domain.ValueObjects;

public record ProcessPaymentCommand(
    int OrderId,
    PaymentMethod PaymentMethod,
    string? CardToken = null,
    int? InstallmentCount = null
) : IRequest<Result<PaymentDto>>;

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, Result<PaymentDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentGatewayService _paymentGateway;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProcessPaymentCommandHandler(
        IOrderRepository orderRepository,
        IPaymentRepository paymentRepository,
        IPaymentGatewayService paymentGateway,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
        _paymentGateway = paymentGateway;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaymentDto>> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        // Get order
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order == null)
        {
            return new Result<PaymentDto>.Failure("Order not found");
        }

        if (order.Status != OrderStatus.Pending)
        {
            return new Result<PaymentDto>.Failure("Order must be pending to process payment");
        }

        // Create payment
        var payment = Domain.Entities.Payment.Create(request.OrderId, request.PaymentMethod, order.TotalAmount);
        await _paymentRepository.AddAsync(payment, cancellationToken);

        // Process payment through gateway (uses ChargeAsync from SAGA-compatible interface)
        var gatewayResult = await _paymentGateway.ChargeAsync(payment, cancellationToken);

        if (!gatewayResult.Success)
        {
            payment.Fail(gatewayResult.ErrorMessage ?? "Payment processing failed");
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new Result<PaymentDto>.Failure(gatewayResult.ErrorMessage ?? "Payment failed");
        }

        // Authorize and capture
        payment.Authorize(gatewayResult.TransactionId!, gatewayResult.GatewayResponse);
        payment.Capture(gatewayResult.GatewayResponse);

        // Confirm order
        order.Confirm();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var paymentDto = _mapper.Map<PaymentDto>(payment);
        return new Result<PaymentDto>.Success(paymentDto);
    }
}

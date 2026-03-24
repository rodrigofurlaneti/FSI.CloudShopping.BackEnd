namespace FSI.CloudShopping.Application.Commands.Payment;

using MediatR;
using FSI.CloudShopping.Domain.Core;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Enums;

public record ProcessManualPaymentCommand(
    int OrderId,
    PaymentMethod PaymentMethod,
    string? Notes = null,
    string ConfirmedByUserId = ""
) : IRequest<Result<PaymentDto>>;

public class ProcessManualPaymentCommandHandler : IRequestHandler<ProcessManualPaymentCommand, Result<PaymentDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProcessManualPaymentCommandHandler(
        IOrderRepository orderRepository,
        IPaymentRepository paymentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaymentDto>> Handle(ProcessManualPaymentCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order == null)
        {
            return new Result<PaymentDto>.Failure("Order not found");
        }

        if (order.Status != OrderStatus.Pending)
        {
            return new Result<PaymentDto>.Failure("Order must be pending to process payment");
        }

        // Create payment with Pending status for manual confirmation
        var payment = Domain.Entities.Payment.Create(request.OrderId, request.PaymentMethod, order.TotalAmount);

        // For manual payments, set a transaction ID indicating it's pending manual confirmation
        payment.Authorize($"MANUAL-{DateTime.UtcNow:yyyyMMddHHmmss}", request.Notes);

        await _paymentRepository.AddAsync(payment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var paymentDto = _mapper.Map<PaymentDto>(payment);
        return new Result<PaymentDto>.Success(paymentDto);
    }
}

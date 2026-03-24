namespace FSI.CloudShopping.Application.Commands.Payment;

using MediatR;
using FSI.CloudShopping.Domain.Core;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;

public record ConfirmManualPaymentCommand(int PaymentId, string ConfirmedByUserId) : IRequest<Result<PaymentDto>>;

public class ConfirmManualPaymentCommandHandler : IRequestHandler<ConfirmManualPaymentCommand, Result<PaymentDto>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ConfirmManualPaymentCommandHandler(
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaymentDto>> Handle(ConfirmManualPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.PaymentId, cancellationToken);
        if (payment == null)
        {
            return new Result<PaymentDto>.Failure("Payment not found");
        }

        // Capture the payment
        payment.Capture($"Confirmed by {request.ConfirmedByUserId}");

        // Get the order and confirm it
        var order = await _orderRepository.GetByIdAsync(payment.OrderId, cancellationToken);
        if (order != null)
        {
            order.Confirm();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var paymentDto = _mapper.Map<PaymentDto>(payment);
        return new Result<PaymentDto>.Success(paymentDto);
    }
}

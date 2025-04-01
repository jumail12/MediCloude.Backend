using Contarcts.Requests.Patient;
using Contarcts.Responses.Patient;
using MassTransit;
using MediatR;
using Razorpay.Api;
using System.ComponentModel.DataAnnotations;


namespace BusinessService.Aplication.Commands.AppoinmentPaymentCommand.Handler
{
    public class RazorPayPaymentCreateCommandHandler : IRequestHandler<RazorPayPaymentCreateCommand, string>
    {
        private readonly IRequestClient<PatientByIdRequest> _requestClient;
        public RazorPayPaymentCreateCommandHandler(IRequestClient<PatientByIdRequest> requestClient)
        {
            _requestClient = requestClient;
        }

        public async Task<string> Handle(RazorPayPaymentCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var RabbitMqRes = await _requestClient.GetResponse<PatientByIdResponse>(new PatientByIdRequest(request.userId));

                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with RabbitMQ.");
                }

                var patient = RabbitMqRes.Message;

                if (patient == null)
                {
                    throw new Exception("Patient not found.");
                }

                var order_id = await RazorPayAppoinmentCreate(request.price);

                if (string.IsNullOrEmpty(order_id))
                {
                    throw new Exception("Failed to create Razorpay order.");
                }

                return order_id;

            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        private async Task<string> RazorPayAppoinmentCreate(long price)
        {
            try
            {
                Dictionary<string, object> input = new Dictionary<string, object>();
                Random random = new Random();
                string transactionId = random.Next(0, 1000).ToString();

                input.Add("amount", Convert.ToDecimal(price) * 100);
                input.Add("currency", "INR");
                input.Add("receipt", transactionId);


                string key = Environment.GetEnvironmentVariable("RazorpayKeyId");
                string secret = Environment.GetEnvironmentVariable("RazorpayKeySecret");

                RazorpayClient client = new RazorpayClient(key, secret);
                Razorpay.Api.Order order = client.Order.Create(input);
                var orderId = order["id"].ToString();

                return orderId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in RazorPayOrderCreate: {ex.Message}");
            }
        }
    }

   
}

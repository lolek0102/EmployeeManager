using EmployeeManager.Application.gRPC;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManager.Client.Pages.gRPC
{
    public class GetEmployeeViaGrpc : PageModel
    {
        [BindProperty]
        public int EmployeeId { get; set; }

        public EmployeeReply? EmployeeInfo { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7291");
            var client = new EmployeeService.EmployeeServiceClient(channel);

            var request = new EmployeeRequest { Id = EmployeeId };

            try
            {
                EmployeeInfo = await client.GetEmployeeInfoAsync(request);
                ErrorMessage = null;
            }
            catch (RpcException ex)
            {
                EmployeeInfo = null;
                ErrorMessage = "Employee not found.";
            }

            return Page();
        }
    }
}

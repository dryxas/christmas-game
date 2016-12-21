namespace Christmas2016.Service.Controllers
{
    using Helpers;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Newtonsoft.Json;
    using Repositories;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api")]
    public class BlobController : ControllerBase
    {
        private readonly IBlobRepository blobRepository;

        public BlobController(IBlobRepository blobRepository)
        {
            this.blobRepository = blobRepository;
        }

        [HttpGet("blobs")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                this.HttpContext.Response.Headers.Add("Content-Type", "text/event-stream");

                // Yaffle polyfill (see: https://github.com/Yaffle/EventSource) for IE requires 2KB padding.

                var padding = new string(' ', 2048 - 34);
                await this.HttpContext.Response.WriteAsync($": Let's get it started! (In here!){padding}\n\n");

                await this.HttpContext.Response.WriteAsync("retry: 1000\n\n");
                await this.GetInternalAsync();
                await this.HttpContext.Response.WriteAsync("event: close\ndata:\n\n");

                return new EmptyResult();
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (System.Exception exception)
            {
                throw new ServiceException(exception);
            }
        }

        private async Task GetInternalAsync()
        {
            int? lastHashCode = null;
            while (!this.HttpContext.RequestAborted.IsCancellationRequested)
            {
                await Task.Delay(20);

                var blobs = await this.blobRepository.GetAsync();
                if (blobs.Count != 0)
                {
                    var hashCode = blobs.GetHashCode();
                    // if (!lastHashCode.HasValue || hashCode != lastHashCode.Value)
                    // {
                        var data = JsonConvert.SerializeObject(blobs.Where(a => !a.Rectangle.IsEmpty).Select(a =>
                            new BlobIndexResponseModel(
                                a.Rectangle.Left + a.Rectangle.Width / 2,
                                a.Rectangle.Top + a.Rectangle.Height / 2)).ToList());
                        await this.HttpContext.Response.WriteAsync($"event: change\ndata: {data}\n\n");
                        await this.HttpContext.Response.Body.FlushAsync();

                        lastHashCode = hashCode;
                    // }
                }

                // Legacy proxy servers are known to, in certain cases, drop HTTP connections after a short
                // timeout. To protect against such proxy servers, a comment line must be included.

                await this.HttpContext.Response.WriteAsync($": runnin' runnin' and runnin' runnin' and\n\n");
            }
        }
    }
}

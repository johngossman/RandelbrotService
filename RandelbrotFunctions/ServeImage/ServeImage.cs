using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Threading.Tasks;
using Randelbrot;

namespace RandelbrotFunctions
{
    public class ServeImage
    {
        private static AutoBrotService autoBrotService = new AutoBrotService(new MandelbrotSet(-0.75, 0.0, 2.5), new DefaultBeautyEvaluator());
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"C# ServeImage triggered. RequestUri={req.RequestUri}");

            MandelbrotRenderer renderer = new MandelbrotRenderer();

            MandelbrotSet set = autoBrotService.PopAndGenerate();

            PixelBuffer buffer = new PixelBuffer(400, 400);
            renderer.RenderToBuffer(set, buffer);

            var utils = new ImageUtils();

            byte[] bits = utils.PixelBufferToJPEG(buffer);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(bits);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

            return response;
        }
    }
}
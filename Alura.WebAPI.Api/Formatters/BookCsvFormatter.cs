using Alura.ListaLeitura.Modelos;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.Api.Formatters
{
    public class BookCsvFormatter : TextOutputFormatter
    {
        public BookCsvFormatter()
        {
            var textCsvMediaType = MediaTypeHeaderValue.Parse("text/csv");
            var appCsvMediaType = MediaTypeHeaderValue.Parse("application/csv");
            SupportedMediaTypes.Add(textCsvMediaType);        
            SupportedMediaTypes.Add(appCsvMediaType);
            SupportedEncodings.Add(Encoding.UTF8);
        }

        protected override bool CanWriteType(System.Type type)
        {
            return type == typeof(LivroApi);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {           
            var bookInCsv = "";

            if (context.Object is LivroApi)
            {
                var book = context.Object as LivroApi;

                bookInCsv = $"/{book.Titulo};{book.Subtitulo};{book.Autor};{book.Lista}/";
            }

            using (var writer = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding))
            {
                return writer.WriteAsync(bookInCsv);
            }//writer.Close()                
        }
    }
}

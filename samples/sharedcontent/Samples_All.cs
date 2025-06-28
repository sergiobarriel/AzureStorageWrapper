using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using samples.Helpers;

namespace samples
{
    public class Samples_All
    {
        private readonly Upload_Blobs _upload_Blobs;
        private readonly Virtual_Folders _virtual_Folders;
        private readonly Delete_Blobs _delete_Blobs;
        private readonly Download_Blobs _download_Blobs;
        private readonly Enumerate_Blobs _enumerate_Blobs;
        public Samples_All(Upload_Blobs upload_Blobs, Virtual_Folders virtual_Folders,Delete_Blobs delete_Blobs,
            Download_Blobs download_Blobs, Enumerate_Blobs enumerate_Blobs)
        {
            _upload_Blobs = upload_Blobs;
            _virtual_Folders = virtual_Folders;
            _delete_Blobs = delete_Blobs;
            _download_Blobs = download_Blobs;
            _enumerate_Blobs = enumerate_Blobs;
        }

        public async Task RunAsync()
        {
            ConsoleHelper.Info("Samples - All");
            await _upload_Blobs.RunAllAsync();
            await _virtual_Folders.RunAllAsync();
            await _download_Blobs.RunAllAsync();
            await _delete_Blobs.RunAllAsync();
            await _enumerate_Blobs.RunAllAsync();
            ConsoleHelper.Info("");
            ConsoleHelper.Info("FIN - Samples - All");
        }
    }

    public static class Example_All_DependencyInjection
    {
        public static IServiceCollection AddExample_All(this IServiceCollection services)
        {
            services.AddScoped<Samples_All>()
                    .AddScoped<Upload_Blobs>()
                    .AddScoped<Virtual_Folders>()
                    .AddScoped<Download_Blobs>()
                    .AddScoped<Delete_Blobs>()
                    .AddScoped<Enumerate_Blobs>()
                    ;
            return services;
        }
    }

}

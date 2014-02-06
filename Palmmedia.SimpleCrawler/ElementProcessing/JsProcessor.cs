
namespace Palmmedia.SimpleCrawler.ElementProcessing
{
    internal class JsProcessor : ElementProcessorBase
    {
        public JsProcessor(Url url, System.Action<Url> actionOnNewUrl, byte[] data)
            : base(url, actionOnNewUrl, data)
        {
        }

        public override void Process(string targetDirectory)
        {
            this.Save(targetDirectory, this.Data);
        }
    }
}

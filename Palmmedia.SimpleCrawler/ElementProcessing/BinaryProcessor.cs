
namespace Palmmedia.SimpleCrawler.ElementProcessing
{
    internal class BinaryProcessor : ElementProcessorBase
    {
        public BinaryProcessor(Url url, System.Action<Url> actionOnNewUrl, byte[] data)
            : base(url, actionOnNewUrl, data)
        {
        }

        public override void Process(string targetDirectory)
        {
            this.Save(targetDirectory, this.Data);
        }
    }
}

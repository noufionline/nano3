using Prism.Commands;

namespace Jasmine.Core
{
    public static class ApplicationCommands
    {
        static ApplicationCommands() => NavigateCommand = new CompositeCommand();

        public static CompositeCommand NavigateCommand { get; private set; }
    }
}
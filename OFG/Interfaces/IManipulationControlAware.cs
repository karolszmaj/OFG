namespace OFG.Interfaces
{
    using System.Windows;
    using System.Windows.Media;

    public interface IManipulationControlAware
    {
        /// <summary>
        /// Target for example small elipse on the bottom that will be hooked for manipulation detals
        /// </summary>
        FrameworkElement ManipulationHandler { get; }

        /// <summary>
        /// All manipulations from ManipulationHandler will be reflected to this RootTransform
        /// </summary>
        CompositeTransform RootTransform { get; }

        Point GetRootControlCenterPoint();

        Point GetManipulationControlCenterPoint();
    }
}

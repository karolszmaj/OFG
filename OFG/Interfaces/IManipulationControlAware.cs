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
        /// This is required for touch calculations from original control
        /// </summary>
        FrameworkElement RootControl { get; }

        double RotationOffset { get; }

        Point GetRootControlCenterPoint();

        Point GetManipulationControlCenterPoint();
    }
}

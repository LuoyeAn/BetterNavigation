using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetterNavigation.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(ContentPageRenderer))]
namespace BetterNavigation.iOS
{
    public class ContentPageRenderer : PageRenderer
    {
        ToolbarTracker _tracker = new ToolbarTracker();
        private Page Child => Element as Page;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _tracker.Target = Child;
            _tracker.AdditionalTargets = Child.GetParentPages();
            _tracker.CollectionChanged += TrackerOnCollectionChanged;

            UpdateToolbarItems();
        }

        void TrackerOnCollectionChanged(object sender, EventArgs eventArgs)
        {
            UpdateToolbarItems();
        }


        private async System.Threading.Tasks.Task UpdateToolbarItems()
        {
            await System.Threading.Tasks.Task.Delay(20);
            var contentPage = this.Element as ContentPage;
            if (contentPage == null || NavigationController == null)
                return;
            var itemsInfo = contentPage.ToolbarItems;

            var navigationItem = this.NavigationController.TopViewController.NavigationItem;

            var newLeftButtons = new UIBarButtonItem[] { }.ToList();
            var newRightButtons = new UIBarButtonItem[] { }.ToList();

            foreach (var item in itemsInfo)
            {
                if (item.Priority == 0)
                    newLeftButtons.Add(item.ToUIBarButtonItem());
                else
                    newRightButtons.Add(item.ToUIBarButtonItem());
            }

            navigationItem.RightBarButtonItems = newRightButtons.ToArray();
            navigationItem.LeftBarButtonItems = newLeftButtons.ToArray();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Child.SendDisappearing();

                _tracker.Target = null;
                _tracker.CollectionChanged -= TrackerOnCollectionChanged;
                _tracker = null;

                if (NavigationItem.RightBarButtonItems != null)
                {
                    for (var i = 0; i < NavigationItem.RightBarButtonItems.Length; i++)
                        NavigationItem.RightBarButtonItems[i].Dispose();
                }

                if (ToolbarItems != null)
                {
                    for (var i = 0; i < ToolbarItems.Length; i++)
                        ToolbarItems[i].Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
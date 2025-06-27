using System.ComponentModel;
using System.Linq.Expressions;
using CommunityToolkit.Maui;
using PetVideoApp.Helpers;

namespace PetVideoApp.Extensions;

public static class Extensions
{
    public static IEnumerable<VisualElement> Ancestors(this VisualElement element)
    {
        while(element != null)
        {
            yield return element;
            element = element.Parent as VisualElement;
        }
    }
    
   
}
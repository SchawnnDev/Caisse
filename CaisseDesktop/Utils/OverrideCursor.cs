using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace CaisseDesktop.Utils
{
    public class OverrideCursor : IDisposable
    {
        private static readonly Stack<Cursor> s_Stack = new Stack<Cursor>();

        public OverrideCursor(Cursor changeToCursor)
        {
            s_Stack.Push(changeToCursor);

            if (Mouse.OverrideCursor != changeToCursor)
                Mouse.OverrideCursor = changeToCursor;
        }

        public void Dispose()
        {
            s_Stack.Pop();

            var cursor = s_Stack.Count > 0 ? s_Stack.Peek() : null;

            if (Mouse.OverrideCursor != null && cursor != Mouse.OverrideCursor)
                Mouse.OverrideCursor = cursor;
        }
    }
}
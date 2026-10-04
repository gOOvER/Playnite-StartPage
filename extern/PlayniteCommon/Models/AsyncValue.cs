using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PlayniteCommon.Models
{
    public class AsyncValue<T> : ObservableObject
    {
        protected Func<Task<T>> Generator { get; }

        protected Dispatcher Dispatcher { get; }

        public AsyncValue(Func<Task<T>> generator, Dispatcher dispatcher = null)
        {
            Generator = generator;
            Dispatcher = dispatcher ?? Application.Current.Dispatcher;
        }

        private bool generated = false;

        private bool isGenerating = false;
        public bool IsGenerating
        {
            get => isGenerating;
            set => SetValue(ref isGenerating, value);
        }

        private T _value = default;

        public T Value
        {
            get 
            {
                if (!generated)
                {
                    generated = true;
                    IsGenerating = true;
                    Dispatcher?.Invoke(async () =>
                    {
                        Value = await Generator();
                    });
                }
                return _value;
            }
            set
            {
                SetValue(ref _value, value);
                IsGenerating = false;
            }
        }
    }
}

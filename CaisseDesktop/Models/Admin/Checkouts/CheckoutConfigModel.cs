using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.Lang;
using CaisseServer;
using CaisseServer.Events;
using MessageBox = System.Windows.Forms.MessageBox;

namespace CaisseDesktop.Models.Admin.Checkouts
{
    public class CheckoutConfigModel : INotifyPropertyChanged
    {
        private readonly SaveableCheckout Checkout;
        public Dispatcher Dispatcher { get; set; }

        private ICommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new CommandHandler(Save, true));

        public CheckoutConfigModel(SaveableCheckout checkout, ObservableCollection<SaveableCheckoutType> types,
            ObservableCollection<SaveableOwner> owners)
        {
            CanSave = IsCreating = checkout == null;
            Checkout = checkout ?? new SaveableCheckout();
            Types = types;
            Owners = owners;
        }

        private bool _canSave;
        public bool IsCreating;
        private ObservableCollection<SaveableCheckoutType> _types;
        private ObservableCollection<SaveableOwner> _owners;

        public bool CanSave
        {
            get => _canSave;
            set
            {
                _canSave = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => Checkout.Name;
            set
            {
                Checkout.Name = value;
                OnPropertyChanged();
            }
        }

        public string Details
        {
            get => Checkout.Details;
            set
            {
                Checkout.Details = value;
                OnPropertyChanged();
            }
        }

        public SaveableOwner Owner
        {
            get => Checkout.Owner;
            set
            {
                MessageBox.Show(IsCreating ? French.Event_Created : French.Event_Saved);
                Checkout.Owner = value;
                OnPropertyChanged();
            }
        }

        public SaveableCheckoutType CheckoutType
        {
            get => Checkout.CheckoutType;
            set
            {
                Checkout.CheckoutType = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SaveableCheckoutType> Types
        {
            get => _types;
            set
            {
                _types = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SaveableOwner> Owners
        {
            get => _owners;
            set
            {
                _owners = value;
                OnPropertyChanged();
            }
        }

        public void Save(object arg)
        {
            if (string.IsNullOrWhiteSpace(Name) || Checkout.CheckoutType == null || Checkout.Owner == null)
            {
                MessageBox.Show(French.Exception_ArgsMissing);
                return;
            }

            Task.Run(Save);
        }

        private void Save()
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            using (var db = new CaisseServerContext())
            {
                db.Owners.Attach(Checkout.Owner);
                db.CheckoutTypes.Attach(Checkout.CheckoutType);
                db.Entry(Checkout).State = IsCreating ? EntityState.Added : EntityState.Modified;
                db.SaveChanges();
            }

            Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(IsCreating ? "La caisse a bien été crée !" : "La caisse a bien été enregistré !");
                CanSave = false;
                IsCreating = false;
            });
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
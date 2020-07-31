using System;
using RentC.WebUI.Infrastructure.Abstract;
using RentC.WebUI.Infrastructure.Concrete;
using RentC.WebUI.Models.Security;
using RentC.WebUI.Models.DAL;

using Unity;
using Unity.AspNet.Mvc;
using System.Web.Mvc;
using Unity.Injection;
using System.Collections.Generic;
using System.Web.Security;

namespace RentC.WebUI
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<Repository<User>, DefaultUserRepository>();
            container.RegisterType<Repository<Role>, DefaultRoleRepository>();
            container.RegisterType<Repository<Car>, DefaultCarRepository>();
            container.RegisterType<Repository<Reservation>, DefaultReservationRepository>();
            container.RegisterType<Repository<ReservationStatus>, DefaultReservationStatusRepository>();
            container.RegisterType<Repository<Coupon>, DefaultCouponRepository>();
            container.RegisterType<Repository<Customer>, DefaultCustomerRepository>();
            container.RegisterType<Repository<Location>, DefaultLocationRepository>();
            container.RegisterType<Repository<Permission>, DefaultPermissionRepository>();

            container.RegisterType<IAuthProvider, WebAuthProvider>();
            container.RegisterType<MembershipProvider, CustomMembershipProvider>();

            container.RegisterType<IRequestTransmitter, DefaultRequestTransmitter>(
                new InjectionProperty("Kword_ControllerDictionary", new Dictionary<string, string>()
                {
                    { "CUST", "Customer"},
                    { "RESERV", "Reservation"},
                    { "CARS", "Car"},
                    { "COUPON", "Coupon" }
                }),
                new InjectionProperty("Kword_ActionDictionary", new Dictionary<string, string>()
                {
                    { "001", "Register"},
                    { "002", "List"},
                    { "003", "Edit"},
                    { "004", "Remove" }
                })
            );


            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
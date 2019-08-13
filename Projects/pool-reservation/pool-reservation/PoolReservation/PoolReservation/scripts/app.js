/// <reference path="C:\Users\rob_d\OneDrive\Documents\pool-reservation\PoolReservation\PoolReservation\views/pages/dashboard/form.html" />
/// <reference path="C:\Users\rob_d\OneDrive\Documents\pool-reservation\PoolReservation\PoolReservation\views/pages/dashboard/form.html" />
'use strict';

/**
* @ngdoc overview
* @name MaterialApp
* @description
* # MaterialApp
*
* Main module of the application.
*/
window.app_version = 2.0;

angular
.module('MaterialApp', [
    'ui.router',
    'ngAnimate',
    'ngMaterial',
    'chart.js',
    'gridshore.c3js.chart',
    'angular-growl',
    'growlNotifications',
    'angular-loading-bar',
    'easypiechart',
    'ui.sortable',
    'PoolReserve',
    'PoolReserve.Login',
    angularDragula(angular),
    'bootstrapLightbox',
    'materialCalendar',
    'paperCollapse',
    'pascalprecht.translate',
    'PoolReserve.Material',
    "PoolReserve.Image",
    "PoolReserve.Hotel",
    "PoolReserve.Venue",
    "PoolReserve.Item",
    "PoolReserve.Reservation",
    "PoolReserve.User"

])
    .config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.latencyThreshold = 5;
        cfpLoadingBarProvider.includeSpinner = false;
    }])
    .config(function ($translateProvider) {
        $translateProvider.useStaticFilesLoader({
            prefix: 'languages/',
            suffix: '.json'
        });
        $translateProvider.useSanitizeValueStrategy(null);
        $translateProvider.preferredLanguage('en');
    })
    .constant("PoolReserveConstants", {
        Pages: {
            Base: {
                Profile: { State: "profile", Url: "views/pages/Profile.html" }
            },
            Authentication: {
                Login: { State: "login", Url: "views/pages/login.html" },
                Logout: { State: "logout", Url: "views/pages/logout.html" },
                Register: { State: "signup", Url: "views/pages/signup.html" },
                Revalidate: { State: "revalidate", Url: "views/pages/revalidate.html" },
                ForgotPassword: { State: "forgotpassword", Url: "views/pages/forgotPassword.html" },
                SendForgotPassword: { State: "sendforgotpassword", Url: "views/pages/sendforgotPassword.html" }
                
            },
            Hotel: {
                AddHotel: { State: "addhotel", Url: "views/pages/addhotel.html" },
                ManageHotels: { State: "managehotels", Url: "views/pages/ManageHotels.html" },
                ManageHotel: { State: "managehotel", Url: "views/pages/ManageHotel.html" }
            },
            Venue: {
                AddVenue: { State: "addvenue", Url: "views/pages/addvenue.html" },
                ManageVenues: { State: "managevenues", Url: "views/pages/ManageVenues.html" }
            }
        },
        Templates: {
            Hotel: {
                ManageHotels: "views/pages/templates/ManageHotels.html",
                ManageHotel: "views/pages/templates/ManageHotel.html",
                ManageEmployees: "views/pages/templates/ManageEmployees.html",
                ManageEmployeeDetails: "views/pages/templates/ManageEmployeeDetails.html"
            },
            Venue: {
                ManageVenues: "views/pages/templates/ManageVenues.html",
                ManageVenue: "views/pages/templates/ManageVenue.html",
                ManageBlackouts: "views/pages/templates/ManageBlackouts.html",
                ModifyBlackout: "views/pages/templates/ModifyBlackout.html"
            },
            Item: {
                ManageItems: "views/pages/templates/ManageItems.html",
                ManageItem: "views/pages/templates/ManageItem.html",
                ManageItemQuantities: "views/pages/templates/ManageItemQuantities.html"
            },
            Image: {
                ManageIcons: "views/pages/templates/ManageIcons.html",
                IconPicker: "views/pages/templates/IconSelectorPanel.html"
            },
            Reservation: {
                ManageReservations: "views/pages/templates/ManageReservations.html"
            },
            User: {
                UserSelector: "views/pages/templates/UserSelectorPanel.html"
            }
        },
        Widgets: {
            Hotel: {
                ManageHotels: { Id: "manageHotels", Invoker: "manage-hotels" },
                ManageHotel: { Id: "manageHotel", Invoker: "manage-hotel" },
                ManageEmployees: { Id: "manageEmployees", Invoker: "manage-employees" },
                ManageEmployeeDetails: { Id: "manageEmployeeDetails", Invoker: "manage-employee-details" }
            },
            Venue: {
                ManageVenues: { Id: "manageVenues", Invoker: "manage-venues" },
                ManageVenue: { Id: "manage-venue", Invoker: "manage-venue" },
                ManageBlackouts: { Id: "manageBlackouts", Invoker: "manage-blackouts" },
                ModifyBlackout: { Id: "modifyBlackout", Invoker: "modify-blackout" }
            },
            Item: {
                ManageItems: { Id: "manageItems", Invoker: "manage-items" },
                ManageItem: { Id: "manageItem", Invoker: "manage-item" },
                ManageItemQuantities: { Id: "manageItemQuantities", Invoker: "manage-item-quantities" }
            },
            Image: {
                ManageIcons: { Id: "manageIcons", Invoker: "manage-icons" },
                IconPicker: {Id: "iconPicker", Invoker:"icon-picker"}
            },
            Reservation: {
                ManageReservations: { Id: "manageReservations", Invoker: "manage-reservations" }
            },
            User: {
                UserSelector: { Id: "userSelector", Invoker: "user-selector" }
            }
        },
        Notifications: {
            Authentication: {
                LOGIN_STATUS_CHANGED: "PoolReserveLoginStatusChanged"
            }

        }
    })
    .config(["$stateProvider", "$urlRouterProvider", "PoolReserveConstants", function ($stateProvider, $urlRouterProvider, PoolReserveConstants) {
        'use strict';

        $stateProvider
        .state('base', {
            abstract: true,
            url: '',
            templateUrl: 'views/base.html?v=' + window.app_version
        })
        .state(PoolReserveConstants.Pages.Authentication.Login.State, {
            url: '/auth/login',
            parent: 'base',
            templateUrl: PoolReserveConstants.Pages.Authentication.Login.Url
        })
        .state(PoolReserveConstants.Pages.Authentication.Logout.State, {
            url: '/auth/logout',
            parent: 'base',
            templateUrl: PoolReserveConstants.Pages.Authentication.Logout.Url
        })
        .state(PoolReserveConstants.Pages.Authentication.Register.State, {
            url: '/auth/signup',
            parent: 'base',
            templateUrl: PoolReserveConstants.Pages.Authentication.Register.Url
        })
        .state(PoolReserveConstants.Pages.Authentication.Revalidate.State, {
            url: '/auth/revalidate',
            parent: 'base',
            templateUrl: PoolReserveConstants.Pages.Authentication.Revalidate.Url
        })
        .state(PoolReserveConstants.Pages.Authentication.ForgotPassword.State, {
            url: '/auth/forgotpassword',
            parent: 'base',
            templateUrl: PoolReserveConstants.Pages.Authentication.ForgotPassword.Url
        })
        .state(PoolReserveConstants.Pages.Authentication.SendForgotPassword.State, {
            url: '/auth/sendforgotpassword',
            parent: 'base',
            templateUrl: PoolReserveConstants.Pages.Authentication.SendForgotPassword.Url
        })
        .state(PoolReserveConstants.Pages.Base.Profile.State, {
                url: '/profile',
                parent: 'dashboard',
                templateUrl: PoolReserveConstants.Pages.Base.Profile.Url
        })
        .state('dashboard', {
            url: '/dashboard',
            parent: 'base',
            templateUrl: 'views/layouts/dashboard.html?v=' + window.app_version
        })
        //.state(PoolReserveConstants.Pages.Base.Home.State, {
        //    url: '/home',
        //    parent: 'dashboard',
        //    templateUrl: PoolReserveConstants.Pages.Base.Home.Url,
        //    controller: 'HomeCtrl'
        //})
        .state(PoolReserveConstants.Pages.Hotel.ManageHotels.State, {
            url: '/managehotels',
            parent: 'dashboard',
            templateUrl: PoolReserveConstants.Pages.Hotel.ManageHotels.Url
        }).state("otherwise", {
            url: "*path",
            template: "",
            controller: [
                      '$state',
              function ($state) {
                  $state.go('managehotels')
              }]
        });

        //$urlRouterProvider.otherwise('/dashboard/home');
    }])
    .run(["$rootScope", "$location", "$state", "LoginService", "PoolReserveConstants", "RedirectService", function ($rootScope, $location, $state, LoginService, PoolReserveConstants, RedirectService) {

        $rootScope.$on("$stateChangeStart", function (event, next, current) {

            var isAuthenticated = LoginService.IsUserLoggedIn();

            var currentState = next.url;
            var currentUrl = $location.url();

            RedirectService.AddUrl(currentUrl);
            //&& !currentUrl.startsWith("/auth"))
            if (isAuthenticated == false && (!currentState.startsWith("/auth"))) {
                RedirectService.SetSpecialUrl($location.url());
                event.preventDefault();
                var theState = PoolReserveConstants.Pages.Authentication.Revalidate.State;
                $state.go(theState);
            }
        });
    }])
    .run(["$rootScope", "$location", "$state", "LoginService", "PoolReserveConstants", "$interval", function ($rootScope, $location, $state, LoginService, PoolReserveConstants, $interval) {
        LoginService.Revalidate();

        var failedAttempts = 0;
        var isRunning = 0;

        $interval(function () {
            if (isRunning != 0 || failedAttempts > 3) {
                return;
            }
            isRunning++;

            LoginService.Revalidate().then(function () {
            }, function () {
                failedAttempts++;
            })

            isRunning--;
        }, 60000);
    }]);



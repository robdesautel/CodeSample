﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="PoolReservation.index" %>

<!doctype html>
<html class="no-js">
<head>
    <meta charset="utf-8">
    <title>Material Admin Dashboard Angular</title>
    <meta name="description" content="">
    <meta name="viewport" content="initial-scale=1, maximum-scale=1">
    <!-- Place favicon.ico and apple-touch-icon.png in the root directory -->
    <!-- build:css(.tmp) styles/vendor.css -->
    <!-- bower:css -->


    <link rel="stylesheet" href="../bower_components/angular-material/angular-material.css" />
    <link rel="stylesheet" href="../bower_components/angular-dragula/dist/dragula.css" />
    <link rel="stylesheet" href="../bower_components/angular-loading-bar/build/loading-bar.css" />
    <link rel="stylesheet" href="../bower_components/angular-chart.js/dist/angular-chart.css" />
    <link rel="stylesheet" href="../bower_components/c3/c3.css" />
    <link rel="stylesheet" href="../bower_components/perfect-scrollbar/css/perfect-scrollbar.css" />
    <link href="styles/mainstyles.css" rel="stylesheet" />
    <link href="styles/scss/textarea.css" rel="stylesheet" />

    <!-- endbower -->
    <link rel="stylesheet" href="../bower_components/mdi/css/materialdesignicons.css" />
    <link rel="stylesheet" href="../bower_components/angular-bootstrap-lightbox/dist/angular-bootstrap-lightbox.min.css" />
    <link rel="stylesheet" href="../bower_components/material-calendar/dist/angular-material-calendar.css" />
    <!-- endbuild -->
    <!-- build:css(.tmp) styles/main.css -->
    <link rel="stylesheet" href="styles/app-blue.css">
    <!-- endbuild -->
    <!--<link href="css/bootstrap/bootstrap.css" rel="stylesheet" />
    <link href="css/bootstrap/bootstrap-theme.css" rel="stylesheet" />-->
    <link rel="stylesheet" href="bower_components/bootstrap/dist/css/bootstrap.min.css" />

    <!-- build:js scripts/vendor.js -->
    <script src="../bower_components/jquery/dist/jquery.min.js"></script>
    <script src="scripts/extras/modernizr.custom.js"></script>
    <!-- bower:js -->
    <script src="../bower_components/angular/angular.js"></script>
    <script src="../bower_components/angular-route/angular-route.min.js"></script>
    <script src="../bower_components/angular-animate/angular-animate.js"></script>
    <script src="../bower_components/angular-aria/angular-aria.js"></script>
    <script src="../bower_components/angular-messages/angular-messages.js"></script>
    <script src="../bower_components/angular-material/angular-material.js"></script>
    <script src="../bower_components/angular-dragula/dist/angular-dragula.js"></script>
    <script src="../bower_components/angular-growl/build/angular-growl.js"></script>
    <script src="../bower_components/angular-growl-notifications/dist/angular-growl-notifications.js"></script>
    <script src="../bower_components/angular-loading-bar/build/loading-bar.js"></script>
    <script src="../bower_components/angular-ui-sortable/sortable.js"></script>
    <script src="../bower_components/Chart.js/Chart.js"></script>
    <script src="../bower_components/angular-chart.js/dist/angular-chart.js"></script>
    <script src="../bower_components/d3/d3.js"></script>
    <script src="../bower_components/c3/c3.js"></script>
    <script src="../bower_components/c3-angular/c3-angular.min.js"></script>
    <script src="../bower_components/angular-sanitize/angular-sanitize.js"></script>
    <script src="../bower_components/material-calendar/dist/angular-material-calendar.js"></script>
    <script src="../bower_components/perfect-scrollbar/js/perfect-scrollbar.js"></script>
    <script src="../bower_components/angular-ui-router/release/angular-ui-router.js"></script>
    <script src="../bower_components/angular-translate/angular-translate.js"></script>
    <script src="../bower_components/angular-translate-loader-url/angular-translate-loader-url.js"></script>
    <script src="../bower_components/angular-translate-loader-static-files/angular-translate-loader-static-files.js"></script>
    <!-- endbower -->
    <script src="../bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js"></script>
    <script src="../bower_components/angular-bootstrap-lightbox/dist/angular-bootstrap-lightbox.min.js"></script>
    <script src="../bower_components/perfect-scrollbar/js/perfect-scrollbar.jquery.js"></script>
    <script src="../bower_components/jquery.easy-pie-chart/dist/angular.easypiechart.min.js"></script>
    <script src="bower_components/bootstrap/dist/js/bootstrap.min.js"></script>



    <script src="scripts/angular/modules/poolReserveModule.js"></script>
    <script src="scripts/angular/controllers/baseController.js"></script>
    <script src="scripts/angular/services/widgetService.js"></script>
    <script src="scripts/angular/directives/createWidgetDirective.js"></script>
    <script src="scripts/angular/filters/poolReserveDateFilter.js"></script>
    <script src="scripts/angular/services/redirectService.js"></script>




    <script src="scripts/angular/modules/fileModule.js"></script>
    <script src="scripts/angular/factories/OutgoingFile.js"></script>

    <script src="scripts/angular/modules/imageModule.js"></script>
    <script src="scripts/angular/directives/singleFileUploadDirective.js"></script>
    <script src="scripts/angular/services/imageService.js"></script>
    <script src="scripts/angular/directives/ManageIconsDirective.js"></script>
    <script src="scripts/angular/directives/iconPickerDirective.js"></script>

    <script src="scripts/angular/modules/materialModule.js"></script>
    <script src="scripts/angular/services/mdToastService.js"></script>
    <script src="scripts/angular/services/mdpanelService.js"></script>

    <script src="scripts/angular/modules/loginModule.js"></script>
    <script src="scripts/angular/controllers/loginController.js"></script>
    <script src="scripts/angular/controllers/registrationController.js"></script>
    <script src="scripts/angular/services/loginService.js"></script>
    <script src="scripts/angular/controllers/revalidateController.js"></script>
    <script src="scripts/angular/controllers/logoutController.js"></script>
    <script src="scripts/angular/controllers/forgotPasswordController.js"></script>
    <script src="scripts/angular/controllers/sendForgotPasswordController.js"></script>

    <script src="scripts/angular/modules/VenueModule.js"></script>
    <script src="scripts/angular/services/venueService .js"></script>
    <script src="scripts/angular/services/specialMessageService.js"></script>
    <script src="scripts/angular/controllers/VenueController.js"></script>
    <script src="scripts/angular/directives/manageVenuesDirective.js"></script>
    <script src="scripts/angular/directives/manageBlackoutsDirective.js"></script>
    <script src="scripts/angular/directives/modifyBlackoutDirective.js"></script>
    <script src="scripts/angular/directives/manageVenueDirective.js"></script>

    <script src="scripts/angular/modules/addHotelModule.js"></script>
    <script src="scripts/angular/services/hotelService.js"></script>
    <script src="scripts/angular/controllers/addHotelController.js"></script>
    <script src="scripts/angular/controllers/manageHotelsController.js"></script>
    <script src="scripts/angular/directives/manageEmployeesDirective.js"></script>
    <script src="scripts/angular/directives/manageHotelsDirective.js"></script>
    <script src="scripts/angular/directives/manageEmployeeDetailsDirective.js"></script>
    <script src="scripts/angular/directives/manageHotelDirective.js"></script>
    <script src="scripts/angular/controllers/showWidgetAsPanelController.js"></script>

    <script src="scripts/angular/modules/itemModule.js"></script>
    <script src="scripts/angular/directives/manageItemsDirective.js"></script>
    <script src="scripts/angular/services/itemService.js"></script>
    <script src="scripts/angular/directives/manageItemDirective.js"></script>
    <script src="scripts/angular/directives/manageItemQuantitiesDirective.js"></script>

    <script src="scripts/angular/modules/reservationModule.js"></script>
    <script src="scripts/angular/services/reservationService.js"></script>
    <script src="scripts/angular/directives/manageReservationsDirective.js"></script>

    <script src="scripts/angular/modules/userModule.js"></script>
    <script src="scripts/angular/services/userService.js"></script>
    <script src="scripts/angular/directives/userSelectorPanel.js"></script>

    <!-- endbuild -->
    <!-- build:js({.tmp,app}) scripts/scripts.js -->
    <script src="scripts/app.js"></script>

    <!--EXTERNAL SCRIPTS-->
    <script>
        function initGoogleMapsWithAngular() {
            $(document).ready(function () {
                //Update google maps code. 
            });
        };
    </script>

    <script src='<%= $"https://maps.googleapis.com/maps/api/js?key={ConfigurationManager.AppSettings["GoogleMapsKey"]}&libraries=places&callback=initGoogleMapsWithAngular" %>' async defer></script>



    <script src="scripts/extras/progressButton.js"></script>
    <script src="scripts/controllers/login.js"></script>
    <script src="scripts/controllers/dashboard.js"></script>
    <script src="scripts/controllers/alertCtrl.js"></script>
    <script src="scripts/controllers/btnCtrl.js"></script>
    <script src="scripts/controllers/componentCtrl.js"></script>
    <script src="scripts/controllers/dateCtrl.js"></script>
    <script src="scripts/controllers/dialogCtrl.js"></script>
    <script src="scripts/controllers/dropdownCtrl.js"></script>
    <script src="scripts/controllers/modalCtrl.js"></script>
    <script src="scripts/controllers/paginationCtrl.js"></script>
    <script src="scripts/controllers/tooltipCtrl.js"></script>
    <script src="scripts/controllers/progressCtrl.js"></script>
    <script src="scripts/controllers/tabsCtrl.js"></script>
    <script src="scripts/controllers/timepickerCtrl.js"></script>
    <script src="scripts/controllers/sidebarCtrl.js"></script>
    <script src="scripts/controllers/homeCtrl.js"></script>
    <script src="scripts/controllers/chartCtrl.js"></script>
    <script src="scripts/controllers/todoCtrl.js"></script>
    <script src="scripts/controllers/buttonCtrl.js"></script>
    <script src="scripts/controllers/clndrCtrl.js"></script>
    <script src="scripts/controllers/profileCtrl.js"></script>
    <script src="scripts/controllers/cardCtrl.js"></script>
    <script src="scripts/controllers/paperCtrl.js"></script>
    <script src="scripts/controllers/docsCtrl.js"></script>

    <script src="scripts/directives/collapse.js"></script>
    <script src="scripts/directives/stats/stats.js"></script>
    <script src="scripts/directives/relink/relink.js"></script>
    <script src="scripts/directives/to-do-list/to-do.js"></script>
    <!-- endbuild -->
</head>
<body ng-app="MaterialApp" class="extended">

    <!--[if lt IE 7]>
      <p class="browsehappy">You are using an <strong>outdated</strong> browser. Please <a href="http://browsehappy.com/">upgrade your browser</a> to improve your experience.</p>
    <![endif]-->
    <!-- Add your site or application content here -->
    <div>
        <div ui-view></div>
    </div>

    <!-- Google Analytics: change UA-XXXXX-X to be your site's ID -->
    <script>
        /*
              (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
              (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
              m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
              })(window,document,'script','//www.google-analytics.com/analytics.js','ga');

              ga('create', 'UA-XXXXX-X');
              ga('send', 'pageview'); */

    </script>

    <!-- build:js scripts/oldieshim.js -->
    <!--[if lt IE 9]>
    <script src="../bower_components/es5-shim/es5-shim.js"></script>
    <script src="../bower_components/json3/lib/json3.js"></script>
    <![endif]-->
    <!-- endbuild -->

</body>
</html>

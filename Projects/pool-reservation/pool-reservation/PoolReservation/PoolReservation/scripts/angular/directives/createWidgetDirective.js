angular.module('PoolReserve')
  .directive('createWidget', function ($compile) {
      return {
          restrict: 'A',
          replace: true,
          scope: {
              createWidget: '='
          },
          link: function postLink(scope, element, attrs) {
              scope.$watch('createWidget', function (widget) {
                  if (widget == null) {
                      return;
                  }


                  var htmlInvoke = widget.Invoker;


                  var fakeElement = "<" + htmlInvoke + " ";

                  if (widget.Options) {
                      fakeElement += " the-options='createWidget.Options'";
                  }

                  fakeElement += "></" + htmlInvoke + ">";

                  element.html(fakeElement);
                  $compile(element.contents())(scope);



              });
          }
      };
  });
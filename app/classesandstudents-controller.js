angular.module('CASApp', ['smart-table', 'ui.bootstrap'])
    .controller('classCtrl', ['$rootScope', '$scope', '$http', '$uibModal', '$log', 'selectedValues', function ($rootScope, $scope, $http, $uibModal, $log, selectedValues) {
      
        $ctrl = this;

        $ctrl.uibModalInstance = null;

        $ctrl.rowClassCollection = [];

        $ctrl.index = function () {
            $http.get("/api/ClassEntities").then(function (data) {
                $ctrl.rowClassCollection = data.data;
            })
        };

        $ctrl.selectRow = function (row) {

            $ctrl.selectedClassId = row.id;
            selectedValues.setSelectedClassId(row.id);
            $ctrl.selectedClassName = row.className;

            $ctrl.studentsIndex();

        };

        $ctrl.editRow = function(row)
        {
            $ctrl.selectedClassId = row.id;
            selectedValues.setSelectedClassId(row.id);
            $ctrl.selectedClassName = row.className;

            $ctrl.newClassName = row.className;
            $ctrl.newClassLocation = row.location;
            $ctrl.newClassTeacher = row.teacher;

            $ctrl.uibModalInstance = $uibModal.open({
                //animation: $ctrl.animationsEnabled,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'myModalContent.html',
                //controller: 'ModalInstanceCtrl',
                //controllerAs: '$ctrl'
                scope: $scope
                //size: size,
                //appendTo: parentElem,
                /*resolve: {
                    items: function () {
                        return $ctrl.items;
                    }
                }*/
            });
        }

        $ctrl.editStudentRow = function (row) {
            $ctrl.selectedStudentId = row.id;
            selectedValues.setSelectedStudentId(row.id);
            $ctrl.selectedStudentName = row.studentFullName;

            $ctrl.newStudentName = row.studentFullName.split(' ')[0];
            $ctrl.newStudentSurname = row.studentFullName.split(' ')[1];
            $ctrl.newStudentDob = new Date(row.dob);
            $ctrl.newStudentGpa = row.gpa;


            $ctrl.uibModalInstance = $uibModal.open({
                //animation: $ctrl.animationsEnabled,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'myModalStudentContent.html',
                scope: $scope
                //size: size,
                //appendTo: parentElem,
                /*resolve: {
                    items: function () {
                        return $ctrl.items;
                    }
                }*/
            });
        }
        $ctrl.removeStudentRow = function (row) {
            $ctrl.selectedStudentId = row.id;
            selectedValues.setSelectedStudentId(row.id);
            $ctrl.selectedStudentName = row.studentFullName;

            $ctrl.uibModalInstance = $uibModal.open({
                //animation: $ctrl.animationsEnabled,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'myDeleteContent.html',
                scope: $scope
                //size: size,
                //appendTo: parentElem,
                /*resolve: {
                    items: function () {
                        return $ctrl.items;
                    }
                }*/
            });
        }
        $ctrl.studentsIndex = function () {
            $http.get("/api/StudentEntities/" + $ctrl.selectedClassId).then(function (data) {
                $ctrl.rowStudentCollection = data.data;
            })
        };
        
        $ctrl.addClassItem = function addClassItem() {

            $ctrl.uibModalInstance = $uibModal.open({
                //animation: $ctrl.animationsEnabled,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'myModalContent.html',
                scope: $scope
                //size: size,
                //appendTo: parentElem,
                /*resolve: {
                    items: function () {
                        return $ctrl.items;
                    }
                }*/
            });
        }
        
        $ctrl.addStudentItem = function addClassItem() {

            $ctrl.uibModalInstance = $uibModal.open({
                //animation: $ctrl.animationsEnabled,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'myModalStudentContent.html',
                scope: $scope
                //size: size,
                //appendTo: parentElem,
                /*resolve: {
                    items: function () {
                        return $ctrl.items;
                    }
                }*/
            });
        }

        $ctrl.removeRow = function removeRow(row) {
            
            $ctrl.selectedStudentId = null;
            selectedValues.setSelectedStudentId(null);

            $ctrl.selectedStudentName = "";
            $ctrl.selectedClassId = row.id;
            selectedValues.setSelectedClassId(row.id);

            $ctrl.uibModalInstance = $uibModal.open({
                //animation: $ctrl.animationsEnabled,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'myDeleteContent.html',
                scope: $scope
                //size: size,
                //appendTo: parentElem,
                /*resolve: {
                    items: function () {
                        return $ctrl.items;
                    }
                }*/
            });

        }

        $ctrl.index();

        $rootScope.$on('refreshClasses', eventFunc)

        $rootScope.$on('refreshStudents', eventStudsFunc)

        function eventFunc()
        {
            $ctrl.index();
        }
        function eventStudsFunc() {
            $ctrl.studentsIndex($ctrl.selectedClassId);
        }

        //modals

        $ctrl.newClassName = "";
        $ctrl.newClassLocation = "";
        $ctrl.newClassTeacher = "";

        $ctrl.ok = function () {

            var newClassName = $ctrl.newClassName;
            var newClassLocation = $ctrl.newClassLocation;
            var newClassTeacher = $ctrl.newClassTeacher;

            var obj = { className: newClassName, location: newClassLocation, teacher: newClassTeacher };

            //dataacess
            $http.post("/api/ClassEntities", obj).then(function (e) { //success


                $ctrl.newClassName = "";
                $ctrl.newClassLocation = "";
                $ctrl.newClassTeacher = "";
                

                $rootScope.$broadcast('refreshClasses');
                //$ctrl.rowClassCollection.push(obj);
            }, function (e) { //error
                console.log("error");
                console.log(e);
            });

            $ctrl.uibModalInstance.close(obj);
        };

        $ctrl.editClass = function () {

            var selClassId = selectedValues.getSelectedClassId();
            if (selClassId == null)
                return;

            var newClassName = $ctrl.newClassName;
            var newClassLocation = $ctrl.newClassLocation;
            var newClassTeacher = $ctrl.newClassTeacher;

            var obj = { className: newClassName, location: newClassLocation, teacher: newClassTeacher };

            //dataacess
            $http.put("/api/ClassEntities/" + selClassId, obj).then(function (e) { //success
                selectedValues.setSelectedClassId(null);
                $rootScope.$broadcast('refreshClasses');
                //$ctrl.rowClassCollection.push(obj);
            }, function (e) { //error
                console.log("error");
                console.log(e);
                selectedValues.setSelectedClassId(null);
            });

            $ctrl.uibModalInstance.close(obj);
        };

        /*  $ctrl.removeClass = function () {
      
              var selClassId = selectedValues.getSelectedClassId();
              if (selClassId == null)
                  return;
              //dataacess
              $http.delete("/api/ClassEntities/" + selClassId).then(function (e) { //success
                  $rootScope.$broadcast('refreshClasses');
                  //$ctrl.rowClassCollection.push(obj);
              }, function (e) { //error
                  console.log("error");
                  console.log(e);
              });
      
              $ctrl.uibModalInstance.close(obj);
          };*/

        $ctrl.okStudent = function () {

            var selStudentId = selectedValues.getSelectedStudentId();
            if (selStudentId != null)
            {
                $ctrl.editStudent();            
                return;
            }


            var newStudentName = $ctrl.newStudentName;
            var newStudentSurname = $ctrl.newStudentSurname;
            var newStudentDOB = $ctrl.newStudentDob;
            var newStudentGPA = $ctrl.newStudentGpa;

            var obj = { studentName: newStudentName, studentSurname: newStudentSurname, DOB: newStudentDOB, GPA: newStudentGPA };

            var selClassId = selectedValues.getSelectedClassId();
            if (selClassId == null)
                return;

            //dataacess
            $http.post("/api/StudentEntities/Create/" + selClassId, obj).then(function (e) { //success

                $ctrl.newStudentName = "";
                $ctrl.newStudentSurname = "";
                $ctrl.newStudentDob = "";
                $ctrl.newStudentGpa = 0;

                $rootScope.$broadcast('refreshStudents');
                //$ctrl.rowClassCollection.push(obj);
            }, function (e) { //error
                console.log("error");
                console.log(e);
                alert("Error, while adding student. " + e.data.message);
            });

            $ctrl.uibModalInstance.close(obj);
        };


        $ctrl.newStudentName = "";
        $ctrl.newStudentSurname = "";
        $ctrl.newStudentDob = "";
        $ctrl.newStudentGpa = 0;

        $ctrl.editStudent = function () {

            var selStudentId = selectedValues.getSelectedStudentId();
            if (selStudentId == null)
                return;

            var newStudentName = $ctrl.newStudentName;
            var newStudentSurname = $ctrl.newStudentSurname;
            var newStudentDOB = $ctrl.newStudentDob;
            var newStudentGPA = $ctrl.newStudentGpa;

            var obj = { studentName: newStudentName, studentSurname: newStudentSurname, DOB: newStudentDOB, GPA: newStudentGPA };

            var selClassId = selectedValues.getSelectedClassId();
            if (selClassId == null)
                return;

            //dataacess
            $http.put("/api/StudentEntities/" + selStudentId, obj).then(function (e) { //success
                selectedValues.setSelectedStudentId(null);
                $ctrl.newStudentName = "";
                $ctrl.newStudentSurname = "";
                $ctrl.newStudentDob = "";
                $ctrl.newStudentGpa = 0;

                $rootScope.$broadcast('refreshStudents');

                $ctrl.uibModalInstance.close(obj);
                //$ctrl.rowClassCollection.push(obj);
            }, function (e) { //error
                console.log("error");
                console.log(e);

                alert("Error, while updating student. " + e.data.message);
                selectedValues.setSelectedStudentId(null);
            });

        };

        $ctrl.removeOk = function () {

            var selClassId = selectedValues.getSelectedClassId();
            if (selClassId == null)
                return;
            var selStudentId = selectedValues.getSelectedStudentId();
            var isClassRemoving = false;

            if (selStudentId == null)
                isClassRemoving = true;
            //dataacess

            if (isClassRemoving) {
                $http.delete("/api/ClassEntities/" + selClassId).then(function (e) { //success
                    $rootScope.$broadcast('refreshClasses');
                    $rootScope.$broadcast('refreshStudents');
                    $ctrl.selectedClassId = undefined;
                    //$ctrl.rowClassCollection.push(obj);
                }, function (e) { //error
                    console.log("error");
                    console.log(e);
                });

            }
            else {

                $http.delete("/api/StudentEntities/" + selClassId + "/" + selStudentId).then(function (e) { //success
                    $rootScope.$broadcast('refreshClasses');
                    $rootScope.$broadcast('refreshStudents');
                    //$ctrl.rowClassCollection.push(obj);
                }, function (e) { //error
                    console.log("error");
                    console.log(e);
                });

            }
            $ctrl.uibModalInstance.close(true);
        };

        $ctrl.cancel = function () {
            $ctrl.uibModalInstance.dismiss('cancel');
            
            $ctrl.newClassName = "";
            $ctrl.newClassLocation = "";
            $ctrl.newClassTeacher = "";


            $ctrl.newStudentName = "";
            $ctrl.newStudentSurname = "";
            $ctrl.newStudentDob = "";
            $ctrl.newStudentGpa = 0;

        };
    }]);

angular.module('CASApp').service('selectedValues', function() {
    var selectedClassId = null;
    var selectedStudentId = null;

    this.setSelectedClassId = function (x) {
        selectedClassId = x;
    }
    
    this.getSelectedClassId = function () {
        return selectedClassId;
    }

    this.setSelectedStudentId = function (x) {
        selectedStudentId = x;
    }

    this.getSelectedStudentId = function () {
        return selectedStudentId;
    }

   
});
var inventoryApp = angular.module('inventoryApp', []);

inventoryApp.controller('InventoryController', ['$scope', '$http', function ($scope, $http) {
	var init = () => {
		$http.get('/Inventory/GetDuplicateInventoryItems').then((response) => {
			$scope.DuplicateInventoryItems = response.data;
		});
		$http.get('/Inventory/GetDuplicateParts').then((response) => {
			$scope.DuplicateParts = response.data;
		});
		$http.get('/Inventory/GetDuplicateOrders').then((response) => {
			$scope.DuplicateOrders = response.data;
		});
		$http.get('/Inventory/GetDuplicateInventoryLocations').then((response) => {
			$scope.DuplicateInventoryLocations = response.data;
		});
		$http.get('/Inventory/GetOldInventory').then((response) => {
			$scope.OldInventoryCount = response.data;
		});
	};

	init();

	$scope.ShowDate = function (date) {
		var date2 = new Date(parseInt(date.replace('/Date(', '')));

		return date2.toLocaleDateString("en-GB");
	}

	$scope.FixDuplicateItems = function () {
		$scope.InvItemsLoading = true;
		$http.post('/Inventory/FixDuplicateInventoryItems').then((response) => {
			$http.get('/Inventory/GetDuplicateInventoryItems').then((response) => {
				$scope.DuplicateInventoryItems = response.data;
				if (response.data.length) {
					$scope.FixDuplicateItems();
				}
				else {
					$scope.InvItemsLoading = false;
				}
			});
		});
	}

	$scope.FixDuplicateParts = function () {
		$scope.PartsLoading = true;
		$http.post('/Inventory/FixDuplicateParts').then((response) => {
			$http.get('/Inventory/GetDuplicateParts').then((response) => {
				$scope.DuplicateParts = response.data;
				if (response.data.length) {
					$scope.FixDuplicateParts()
				}
				else {
					$scope.PartsLoading = false;
				}
			});
		});
	}

	$scope.FixDuplicateOrders = function () {
		$scope.OrdersLoading = true;
		$http.post('/Inventory/FixDuplicateOrders').then((response) => {
			$http.get('/Inventory/GetDuplicateOrders').then((response) => {
				$scope.DuplicateOrders = response.data;
				if (response.data.length) {
					$scope.FixDuplicateOrders()
				}
				else {
					$scope.OrdersLoading = false;
				}
			});
		});
	}

	$scope.FixOldInventory = function () {
		$scope.OldInvLoading = true;

		const get = () => $http.get('/Inventory/FixOldInventory');

		async function* asyncfunction() {
			var callCount = $scope.OldInventoryCount / 5;
			for (let i = 0; i < callCount; i++) {
				yield get().then((response) => { $scope.OldInventoryCount -= response.data; return i; });
			}
		}

		async function loop() {
			for await (let i of asyncfunction()) {
				console.log(i);
			}

			alert("Done! Reload the page.");
			$scope.OldInvLoading = false;
		}

		loop();
	}
}]);

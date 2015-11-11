function HomeViewModel(app, dataModel) {
    var self = this;

    self.myHometown = ko.observable("");

    Sammy(function () {
        this.get('#home', function () {
            // Make a call to the protected Web API by passing in a Bearer Authorization Header
            $.ajax({
                method: 'get',
                url: app.dataModel.userInfoUrl,
                contentType: "application/json; charset=utf-8",
                headers: {
                    'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
                },
                success: function (data) {
                    self.myHometown('Your Hometown is : ' + data.hometown);
                }
            });
        });
        this.get('/', function () { this.app.runRoute('get', '#home') });
    });

	self.engageTractorBeam = function() {
		$.ajax({
			method: 'post',
			url: '/api/tractor/engage',
			contentType: "application/json; charset=utf-8",
			headers: {
				'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
			},
			success: function (data) {
				alert('engaged!');
			}
		});
	}

	self.openLoadingDoors = function () {
		$.ajax({
			method: 'post',
			url: '/api/doors/set',
			headers: {
				'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
			},
			data: '=open' ,
			success: function (data) {
				alert(data.status);
			}
		});
	}
	self.closeLoadingDoors = function () {
		$.ajax({
			method: 'post',
			url: '/api/doors/set',
			headers: {
				'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
			},
			data: '=close',
			success: function (data) {
				alert(data.status);
			}
		});
	}

    return self;
}

app.addViewModel({
    name: "Home",
    bindingMemberName: "home",
    factory: HomeViewModel
});

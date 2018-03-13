describe("DefineNodeWizardStepController", function () {
    var $httpBackend, $rootScope, createController;
    app.controller("DefineNodeWizardStepController", ["$http", AddNodeWizard.DefineNodeWizardStepController]);
    beforeEach(angular.mock.module("AddNodeWizard"));
    beforeEach(inject(function ($injector) {
        $httpBackend = $injector.get("$httpBackend");
        $rootScope = $injector.get("$rootScope");
        var $controller = $injector.get("$controller");
        createController = function () { return $controller("DefineNodeWizardStepController", { "$scope": $rootScope }); };
    }));
    it("returns false from onNext() when node is invalid", function () {
        var controller = createController();
        controller.setNode(new AddNodeWizard.Node());
        var result = controller.onNext();
        expect(result).toBe(false, "onNext should return false for invalid node");
    });
    it("validates node IP address on onNext()", function () {
        var controller = createController();
        controller.setNode(new AddNodeWizard.Node());
        controller.onNext();
        expect(controller.ipAddressOrHostnameIsValid).toBe(false, "Flag for invalid IP should be set.");
    });
    it("blocks transition on invalid IP address", function () {
        var controller = createController();
        controller.setNode(new AddNodeWizard.Node());
        expect(controller.onNext()).toBe(false, "Invalid IP should block transition to next step");
    });
    it("sets proper error for empty node IP address on onNext()", function () {
        var controller = createController();
        controller.setNode(new AddNodeWizard.Node());
        controller.onNext();
        expect(controller.ipAddressOrHostnameValidationError).toBe("IP Address or hostname must be filled in", "Wrong error set for empty IP");
    });
    it("validates node credentials on onNext()", function () {
        var controller = createController();
        var node = new AddNodeWizard.Node();
        node.IpOrHostname = "1.1.1.1";
        node.PollingMethod = AddNodeWizard.PollingMethod.SNMP;
        node.SnmpCredentials.CommunityString = "";
        controller.setNode(node);
        controller.onNext();
        expect(controller.credentialsAreValid).toBe(false, "Flag for invalid credentials should be set.");
    });
    it("blocks transition on invalid SNMP credentials", function () {
        var controller = createController();
        var node = new AddNodeWizard.Node();
        node.IpOrHostname = "1.1.1.1";
        node.PollingMethod = AddNodeWizard.PollingMethod.SNMP;
        node.SnmpCredentials.CommunityString = "";
        controller.setNode(node);
        expect(controller.onNext()).toBe(false, "Invalid credentials should block transition to next step");
    });
    it("allows transition for valid node", function () {
        var controller = createController();
        var node = new AddNodeWizard.Node();
        node.IpOrHostname = "1.1.1.1";
        node.PollingMethod = AddNodeWizard.PollingMethod.SNMP;
        node.SnmpCredentials.Port = 161;
        node.SnmpCredentials.CommunityString = "public";
        controller.setNode(node);
        expect(controller.onNext()).toBe(true, "Valid node should allow transition to next step");
    });
    it("does not validate node credentials for ICMP node", function () {
        var controller = createController();
        var node = new AddNodeWizard.Node();
        node.IpOrHostname = "1.1.1.1";
        node.PollingMethod = AddNodeWizard.PollingMethod.ICMP;
        controller.setNode(node);
        controller.onNext();
        expect(controller.credentialsAreValid).toBe(true, "Flag for invalid credentials should not be set.");
    });
    it("shows SNMP credentials for SNMP node", function () {
        var controller = createController();
        var node = new AddNodeWizard.Node();
        node.PollingMethod = AddNodeWizard.PollingMethod.SNMP;
        controller.setNode(node);
        controller.nodeTypeChanged();
        expect(controller.showSnmpCredentials).toBe(true, "SNMP credentials should be visible");
    });
    it("does not show WMI credentials for SNMP node", function () {
        var controller = createController();
        var node = new AddNodeWizard.Node();
        node.PollingMethod = AddNodeWizard.PollingMethod.SNMP;
        controller.setNode(node);
        controller.nodeTypeChanged();
        expect(controller.showWmiCredentials).toBe(false, "WMI credentials should not be visible");
    });
    it("shows WMI credentials for WMI node", function () {
        var controller = createController();
        var node = new AddNodeWizard.Node();
        node.PollingMethod = AddNodeWizard.PollingMethod.WMI;
        controller.setNode(node);
        controller.nodeTypeChanged();
        expect(controller.showWmiCredentials).toBe(true, "WMI credentials should be visible");
    });
    it("does not show SNMP credentials for WMI node", function () {
        var controller = createController();
        var node = new AddNodeWizard.Node();
        node.PollingMethod = AddNodeWizard.PollingMethod.WMI;
        controller.setNode(node);
        controller.nodeTypeChanged();
        expect(controller.showSnmpCredentials).toBe(false, "WMI credentials should not be visible");
    });
});
//# sourceMappingURL=DefineNodeWizardStepControllerTest.js.map
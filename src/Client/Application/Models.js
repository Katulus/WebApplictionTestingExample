'use strict';
var AddNodeWizard;
(function (AddNodeWizard) {
    var Node = /** @class */ (function () {
        function Node(ipOrHostname) {
            this.SnmpCredentials = { Port: 161, CommunityString: "public" };
            this.WmiCredentials = { Username: "", Password: "", PasswordConfirm: "" };
            this.IpOrHostname = ipOrHostname;
        }
        Node.prototype.isSnmp = function () {
            return this.PollingMethod === PollingMethod.SNMP;
        };
        Node.prototype.isWmi = function () {
            return this.PollingMethod === PollingMethod.WMI;
        };
        return Node;
    }());
    AddNodeWizard.Node = Node;
    var PollingMethod = /** @class */ (function () {
        function PollingMethod() {
        }
        PollingMethod.ICMP = "ICMP";
        PollingMethod.SNMP = "SNMP";
        PollingMethod.WMI = "WMI";
        return PollingMethod;
    }());
    AddNodeWizard.PollingMethod = PollingMethod;
})(AddNodeWizard || (AddNodeWizard = {}));
//# sourceMappingURL=Models.js.map
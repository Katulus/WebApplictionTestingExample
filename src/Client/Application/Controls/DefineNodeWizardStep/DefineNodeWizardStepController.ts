
module AddNodeWizard {
    export class DefineNodeWizardStepController implements IWizardStep {
        private node: Node;

        public ipAddressOrHostnameIsValid: boolean;
        public ipAddressOrHostnameValidationError: string;

        public credentialsAreValid: boolean;
        public credentialsValidationError: string;

        public showSnmpCredentials: boolean;
        public showWmiCredentials: boolean;

        public setNode(node: Node): void {
            this.node = node;
            this.nodeTypeChanged();
        }

        public onNext(): boolean {
            this.ipAddressOrHostnameIsValid = true;
            this.credentialsAreValid = true;

            this.ipAddressOrHostnameIsValid = this.validateIpOrHostname();
            this.credentialsAreValid = this.validateCredentials();
 
            return this.ipAddressOrHostnameIsValid
                && this.credentialsAreValid;
        }
        private validateIpOrHostname() {
            if (!this.node.IpOrHostname) {
                this.ipAddressOrHostnameIsValid = false;
                this.ipAddressOrHostnameValidationError = 'IP Address or hostname must be filled in';
                return false;
            }
            return true;
        }

        private validateCredentials() {
            switch (this.node.PollingMethod) {
                case PollingMethod.SNMP:
                    return this.validateSnmpCredentials();
                case PollingMethod.WMI:
                    return this.validateWmiCredentials();
                default:
                    return true;
            }
        }

        private validateSnmpCredentials() {
            if (!this.node.SnmpCredentials.Port) {
                this.credentialsValidationError = 'SNMP port number must be filled in';
                return false;
            }

            if (!this.node.SnmpCredentials.Port.toString().match(/^\d+$/)) {
                this.credentialsValidationError = 'SNMP port number must be a number';
                return false;
            }
            if (!this.node.SnmpCredentials.CommunityString) {
                this.credentialsValidationError = 'SNMP community string must be filled in';
                return false;
            }
            return true;
        }

        private validateWmiCredentials() {
            if (!this.node.WmiCredentials.Username) {
                this.credentialsValidationError = 'WMI username must be filled in';
                return false;
            }
            if (!this.node.WmiCredentials.Password) {
                this.credentialsValidationError = 'WMI password must be filled in';
                return false;
            }
            if (this.node.WmiCredentials.Password !== this.node.WmiCredentials.PasswordConfirm) {
                this.credentialsValidationError = 'WMI passwords must be same';
                return false;
            }
            return true;
        }

        public testCredentials(): void {
            switch (this.node.PollingMethod) {
                case PollingMethod.SNMP:
                    // test SNMP credentials
                    break;
                case PollingMethod.WMI:
                    // test WMI credentials
                    break;
            }
        }
 
        // This method is called when radiobutton for node type is changed to refresh UI flags.
        // We could use $scope.watch(...) on this.node.Type as well but this is easier to test.
        public nodeTypeChanged(): void {
            this.showSnmpCredentials = this.node.PollingMethod === PollingMethod.SNMP;
            this.showWmiCredentials = this.node.PollingMethod === PollingMethod.WMI;
        }
    }
}
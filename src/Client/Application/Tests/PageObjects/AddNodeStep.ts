import SummaryStep = require("./SummaryStep");
import Promise = protractor.promise.Promise;

class AddNodeStep {
    private _nextButton = element(by.css('.next'));
    private _addressField = element(by.model('vm.node.IpOrHostname'));
    private _snmpRadio = element(by.id('snmpMethodRadio'));
    private _navigator = element(by.css('.sw-wizard-nav-item'));

    public navigate() {
        browser.get('http://localhost:8081/#/add');
    }

    public isLoaded(): Promise<boolean> {
        return this._navigator.isPresent();
    }

    public next(): SummaryStep {
        this._nextButton.click();
        return new SummaryStep();
    }

    public setAddress(address: string) {
        this._addressField.sendKeys(address);
    }

    public setSnmpMethod() {
        this._snmpRadio.click();
    }

    // ... more methods to manipulate other fields, check errors etc.
}

export = AddNodeStep;
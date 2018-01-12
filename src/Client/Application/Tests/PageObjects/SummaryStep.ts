import Promise = protractor.promise.Promise;

class SummaryStep {
    private _addButton = element(by.css('.add'));

    public addNode() {
        this._addButton.click();
    }

    public isActive(): Promise<boolean> {
        var navElements = element.all(by.css('.sw-wizard-nav-item'));
        return navElements.get(1).getAttribute('class').then(value => {
            return /active/.test(value);
        });
    }
}

export = SummaryStep;
import AddNodeStep = require("./AddNodeStep");
import Promise = protractor.promise.Promise;

class IndexPage {
    private _addNodeButton = element(by.id('addNodeButton'));
    private _deleteAllButton = element(by.id('deleteAllButton'));

    public navigate() {
        browser.get('http://localhost:8081/');
    }

    public addNode(): AddNodeStep {
        this._addNodeButton.click();
        return new AddNodeStep();
    }

    public deleteAll() {
        this._deleteAllButton.click();
    }

    public numberOfNodes(): Promise<number> {
        return element.all(by.repeater('node in vm.nodes')).count();
    }

    public getNodeAddress(index: number): Promise<string> {
        return element.all(by.repeater('node in vm.nodes')).get(index).all(by.tagName('td')).get(1).getText();
    }
}

export = IndexPage;
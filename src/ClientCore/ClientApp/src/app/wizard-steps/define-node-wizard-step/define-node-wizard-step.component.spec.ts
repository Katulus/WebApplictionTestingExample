import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { DefineNodeWizardStepComponent } from './define-node-wizard-step.component';
import { FormsModule } from '@angular/forms';
import { Node, PollingMethod } from "../../models";

describe('DefineNodeWizardStepComponent', () => {
  let component: DefineNodeWizardStepComponent;
  let fixture: ComponentFixture<DefineNodeWizardStepComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DefineNodeWizardStepComponent],
      imports: [
        FormsModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DefineNodeWizardStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it("returns false from onNext() when node is invalid", () => {
    // empty IP is considered invalid
    component.setNode(new Node());

    var result = component.onNext();

    expect(result).toBe(false, "onNext should return false for invalid node");
  });

  it("validates node IP address on onNext()", () => {
    component.setNode(new Node());

    component.onNext();

    expect(component.ipAddressOrHostnameIsValid).toBe(false, "Flag for invalid IP should be set.");
  });

  it("blocks transition on invalid IP address", () => {
    component.setNode(new Node());

    expect(component.onNext()).toBe(false, "Invalid IP should block transition to next step");
  });

  it("sets proper error for empty node IP address on onNext()", () => {
    component.setNode(new Node());

    component.onNext();

    expect(component.ipAddressOrHostnameValidationError).toBe("IP Address or hostname must be filled in", "Wrong error set for empty IP");
  });

  it("validates node credentials on onNext()", () => {
    var node = new Node();
    node.IpOrHostname = "1.1.1.1";
    node.PollingMethod = PollingMethod.SNMP;
    node.SnmpCredentials.CommunityString = "";
    component.setNode(node);

    component.onNext();

    expect(component.credentialsAreValid).toBe(false, "Flag for invalid credentials should be set.");
  });

  it("blocks transition on invalid SNMP credentials", () => {
    var node = new Node();
    node.IpOrHostname = "1.1.1.1";
    node.PollingMethod = PollingMethod.SNMP;
    node.SnmpCredentials.CommunityString = "";
    component.setNode(node);

    expect(component.onNext()).toBe(false, "Invalid credentials should block transition to next step");
  });

  it("allows transition for valid node", () => {
    var node = new Node();
    node.IpOrHostname = "1.1.1.1";
    node.PollingMethod = PollingMethod.SNMP;
    node.SnmpCredentials.Port = 161;
    node.SnmpCredentials.CommunityString = "public";
    component.setNode(node);

    expect(component.onNext()).toBe(true, "Valid node should allow transition to next step");
  });

  it("does not validate node credentials for ICMP node", () => {
    var node = new Node();
    node.IpOrHostname = "1.1.1.1";
    node.PollingMethod = PollingMethod.ICMP;
    component.setNode(node);

    component.onNext();

    expect(component.credentialsAreValid).toBe(true, "Flag for invalid credentials should not be set.");
  });

  it("shows SNMP credentials for SNMP node", () => {
    var node = new Node();
    node.PollingMethod = PollingMethod.SNMP;
    component.setNode(node);

    component.nodeTypeChanged(); // this is triggered by radio button change

    expect(component.showSnmpCredentials).toBe(true, "SNMP credentials should be visible");
  });

  it("does not show WMI credentials for SNMP node", () => {
    var node = new Node();
    node.PollingMethod = PollingMethod.SNMP;
    component.setNode(node);

    component.nodeTypeChanged(); // this is triggered by radio button change

    expect(component.showWmiCredentials).toBe(false, "WMI credentials should not be visible");
  });

  it("shows WMI credentials for WMI node", () => {
    var node = new Node();
    node.PollingMethod = PollingMethod.WMI;
    component.setNode(node);

    component.nodeTypeChanged(); // this is triggered by radio button change

    expect(component.showWmiCredentials).toBe(true, "WMI credentials should be visible");
  });

  it("does not show SNMP credentials for WMI node", () => {
    var node = new Node();
    node.PollingMethod = PollingMethod.WMI;
    component.setNode(node);

    component.nodeTypeChanged(); // this is triggered by radio button change

    expect(component.showSnmpCredentials).toBe(false, "WMI credentials should not be visible");
  });
});

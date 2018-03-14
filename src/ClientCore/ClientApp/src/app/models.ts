export interface WizardStepDefinition {
  Id: string;
  ControlName: string;
  Title: string;
}

export interface IWizardStep {
  onNext(): boolean;
  setNode(node: Node): void;
}

export interface StepTransitionResult {
  CanTransition: boolean;
  ErrorMessage: string;
}

export class Node {
  Id: number;
  IpOrHostname: string;
  PollingMethod: PollingMethod;

  SnmpCredentials: SnmpCredentials = { Port: 161, CommunityString: 'public' };
  WmiCredentials: WmiCredentials = { Username: '', Password: '', PasswordConfirm: '' };

  constructor(ipOrHostname?: string) {
    this.IpOrHostname = ipOrHostname;
  }

  public isSnmp() {
    return this.PollingMethod === PollingMethod.SNMP;
  }

  public isWmi() {
    return this.PollingMethod === PollingMethod.WMI;
  }
}

export interface SnmpCredentials {
  Port: number;
  CommunityString: string;
}

export interface WmiCredentials {
  Username: string;
  Password: string;
  PasswordConfirm: string;
}

export class PollingMethod {
  static ICMP: string = 'ICMP';
  static SNMP: string = 'SNMP';
  static WMI: string = 'WMI';
}

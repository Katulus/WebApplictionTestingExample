import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Node } from '../models';
import { HomeComponent } from './home.component';
import { NodesService } from '../nodes.service';


describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  const nodesServiceMock: any = {
    loadNodes: jasmine.createSpy('loadNodes').and.callFake((callback: (result: Node[]) => void, failCallback) => {
      callback([
        new Node('1.1.1.1'),
        new Node('2.2.2.2')
      ]);
    }),
    deleteAll: jasmine.createSpy('deleteAll')
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        HomeComponent
      ],
      providers: [
        {
          provide: NodesService, useValue: nodesServiceMock
        }
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('loads nodes from web service', () => {
    expect(component.nodes.length).toBe(2, 'Wrong number of nodes returned.');
    expect(component.nodes[0].IpOrHostname).toBe('1.1.1.1');
    expect(component.nodes[1].IpOrHostname).toBe('2.2.2.2');
  });
});

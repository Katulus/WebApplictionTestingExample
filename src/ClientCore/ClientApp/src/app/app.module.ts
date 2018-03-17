import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { WizardComponent } from './wizard/wizard.component';
import { WizardService } from './wizard.service';
import { WizardStepDirective } from './wizard-step.directive';
import { DefineNodeWizardStepComponent } from './wizard-steps/define-node-wizard-step/define-node-wizard-step.component';
import { SummaryWizardStepComponent } from './wizard-steps/summary-wizard-step/summary-wizard-step.component';
import { NodesService } from './nodes.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    WizardComponent,
    WizardStepDirective,
    DefineNodeWizardStepComponent,
    SummaryWizardStepComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'add', component: WizardComponent }
    ])
  ],
  providers: [
    NodesService,
    WizardService
  ],
  bootstrap: [AppComponent],
  entryComponents: [DefineNodeWizardStepComponent, SummaryWizardStepComponent]
})
export class AppModule { }

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { WizardComponent } from './wizard/wizard.component';
import { WizardStepDirective } from './wizard-step.directive';
import { DefineNodeWizardStepComponent } from './wizard-steps/define-node-wizard-step/define-node-wizard-step.component';
import { SummaryWizardStepComponent } from './wizard-steps/summary-wizard-step/summary-wizard-step.component';
import { NodesService } from './nodes.service';
import { WizardService } from './wizard.service';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
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

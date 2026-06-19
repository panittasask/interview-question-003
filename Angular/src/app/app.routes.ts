import { Routes } from '@angular/router';
import { ApproveDocumentComponent } from '../modules/approve/approve-document/approve-document.component';

export const routes: Routes = [
  { path: '', redirectTo: 'approve', pathMatch: 'full' },
  { path: 'approve', component: ApproveDocumentComponent },
  { path: '**', redirectTo: 'approve' },
];

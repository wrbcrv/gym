import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { ActivitesComponent } from './components/group/activities/activities.component';
import { ActivityComponent } from './components/group/activity/activity.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'group/:id',
    component: ActivitesComponent
  },
  {
    path: 'group/:groupId/activity/:activityId',
    component: ActivityComponent
  }
];

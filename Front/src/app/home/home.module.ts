import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { IndexComponent } from './pages/index/index.component';
import {MatButton} from '@angular/material/button';


@NgModule({
  declarations: [
    IndexComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    MatButton
  ]
})
export class HomeModule { }

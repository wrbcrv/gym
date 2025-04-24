import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { GroupService } from '../../../services/group.service';
import { ActivatedRoute } from '@angular/router';
import { ActivityUtilsService } from '../../../services/activity-utils.service';

@Component({
  selector: 'app-activity',
  imports: [
    CommonModule
  ],
  templateUrl: './activity.component.html',
  styleUrl: './activity.component.css'
})
export class ActivityComponent implements OnInit {
  groupId: number = 0;
  activityId: number = 0;
  activity: any = null;
  displayDate: string = '';

  constructor(
    private route: ActivatedRoute,
    private groupService: GroupService,
    private activityUtils: ActivityUtilsService,
  ) { }

  ngOnInit(): void {
    this.groupId = Number(this.route.snapshot.paramMap.get('groupId'));
    this.activityId = Number(this.route.snapshot.paramMap.get('activityId'));

    this.groupService.getActivityById(this.groupId, this.activityId).subscribe(
      (res) => {
        this.activity = res;

        if (this.activity && this.activity.date) {
          this.displayDate = this.activityUtils.formatDisplayDate(this.activity.date);
        }
      }
    );
  }
}

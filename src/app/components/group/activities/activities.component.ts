import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DatePipe } from '@angular/common';
import { GroupRankingComponent } from '../ranking/ranking.component';
import { ActivityGroup, ActivityUtilsService } from '../../../services/activity-utils.service';
import { ActivityService } from '../../../services/activity.service';
import { GroupService } from '../../../services/group.service';

@Component({
  selector: 'app-activities',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    GroupRankingComponent
  ],
  providers: [
    DatePipe
  ],
  templateUrl: './activities.component.html',
  styleUrl: './activities.component.css'
})
export class ActivitesComponent implements OnInit {
  groupId: number = 0;
  group: any;
  activities: any[] = [];
  groups: ActivityGroup[] = [];
  userActivityCounts: { userId: number, fullName: string, count: number }[] = [];
  pageNumber: number = 1;
  pageSize: number = 8;
  loading: boolean = false;
  hasMore: boolean = true;
  skeletonItems = Array(8).fill(0).map((_, i) => i);

  thumbnailUrl: string = 'https://images7.alphacoders.com/133/1330715.png'

  constructor(
    private route: ActivatedRoute,
    private activityService: ActivityService,
    private groupService: GroupService,
    private activityUtils: ActivityUtilsService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.groupId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadGroup();
    this.loadActivities();
  }

  loadGroup(): void {
    this.groupService.getById(this.groupId).subscribe(
      (res) => {
        this.group = res;
      },
      (err) => {
        this.router.navigateByUrl('/');
      }
    );
  }

  loadActivities(): void {
    if (this.loading || !this.hasMore) return;

    this.loading = true;

    this.activityService.getAll(this.groupId, this.pageNumber, this.pageSize).subscribe(
      (res) => {
        if (res.length < this.pageSize) {
          this.hasMore = false;
        }

        this.activities.push(...res);
        this.groups = this.activityUtils.groupActivitiesByDate(this.activities);
        this.pageNumber++;
        this.loading = false;
      },
      (err) => {
        console.error(err);
        this.loading = false;
      }
    );
  }

  onScroll(event: any): void {
    const element = event.target;
    const bottomReached = element.scrollHeight - element.scrollTop <= element.clientHeight + 100;

    if (bottomReached && !this.loading && this.hasMore) {
      this.loadActivities();
    }
  }
  
  get allActivities() {
    return this.groups.flatMap(group => group.activities);
  }
}
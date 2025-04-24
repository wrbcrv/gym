import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ActivityService } from '../../../services/activity.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-ranking',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './ranking.component.html',
  styleUrl: './ranking.component.css'
})
export class GroupRankingComponent implements OnInit {
  @Input() groupId!: number;

  currentUserId: number = 0;
  userActivityCounts: { userId: number, fullName: string, count: number }[] = [];
  isRankingVisible = true;

  constructor(
    private activityService: ActivityService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe(
      (res) => {
        this.currentUserId = res.id;
        this.loadRanking();
      }
    )
  }

  toggleRankingVisibility() {
    this.isRankingVisible = !this.isRankingVisible;
  }

  loadRanking(): void {
    this.activityService.getRanking(this.groupId).subscribe(
      (res) => {
        this.userActivityCounts = res.slice(0, 3);

        while (this.userActivityCounts.length < 3) {
          this.userActivityCounts.push({
            userId: 0,
            fullName: '',
            count: 0
          });
        }
      }
    );
  }

  getMaxCount(): number {
    const maxCount = Math.max(...this.userActivityCounts.map(user => user.count));
    return maxCount > 0 ? maxCount : 1;
  }

  displayName(userId: number, fullName: string): string {
    return userId === this.currentUserId ? 'Você' : fullName || 'Usuário';
  }
}

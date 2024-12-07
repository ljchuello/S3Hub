import {Component, OnInit} from '@angular/core';
import {Helper} from '../../../shared/class/helper';
import {ActivatedRoute, Router} from '@angular/router';
import {RepoService} from '../../../api/services/repo.service';
import {OFile} from '../../../api/models/o-file';
import {lastValueFrom} from 'rxjs';
import {OAccount} from '../../../api/models/o-account';

@Component({
  selector: 'app-index',
  standalone: false,

  templateUrl: './index.component.html',
  styleUrl: './index.component.css'
})
export class IndexComponent implements OnInit {

  helper: Helper = new Helper();

  constructor(
    private activatedRoute: ActivatedRoute,
    private repoService: RepoService,
    private router: Router,
  ) {
  }

  oFileList: OFile[] = [];

  async ngOnInit() {
    try {
      let oAccount: OAccount = {
        accessKey: '002762586be01e70000000024',
        secretKey: 'K0026FjQ/RrJJpvQfSe6haKSLGZHLEc',
        serviceUrl: 's3.us-west-002.backblazeb2.com',
        bucketName: 'ljchuello',
      };

      let directory = this.helper.GetId(this.activatedRoute)!;
      if (!directory!)
        directory! = '/';
      console.log(directory!);
      this.oFileList = await lastValueFrom(this.repoService.apiRepoListObjectsV2Post({
        directory: directory!,
        getInfo: false,
        body: oAccount!,
      }));
    } catch (error) {
      console.error('xd');
    }
  }

  async btnNavegar(file: OFile) {

    let oAccount: OAccount = {
      accessKey: '002762586be01e70000000024',
      secretKey: 'K0026FjQ/RrJJpvQfSe6haKSLGZHLEc',
      serviceUrl: 's3.us-west-002.backblazeb2.com',
      bucketName: 'ljchuello',
    };

    if (file!.mimeType! == '/') {
      this.oFileList = await lastValueFrom(this.repoService.apiRepoListObjectsV2Post({
        directory: file!.key!,
        getInfo: false,
        body: oAccount!,
      }));
    } else {
      let url = await lastValueFrom(this.repoService.apiRepoPreSignedUrlPost({
        key: file!.key!,
        body: oAccount!
      }));
      window.open(url!, '_blank');
    }
  }

}

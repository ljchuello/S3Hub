import {ActivatedRoute} from '@angular/router';

export class Helper {

  GetId(activatedRoute: ActivatedRoute): string {
    if (activatedRoute.snapshot.paramMap.get('id')!)
      return activatedRoute.snapshot.paramMap.get('id')!;
    else
      return '';
  }

}

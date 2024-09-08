import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'logFormatter',
})
export class LogFormatterPipe implements PipeTransform {
  transform(log: string): string {
    return log;
  }
}

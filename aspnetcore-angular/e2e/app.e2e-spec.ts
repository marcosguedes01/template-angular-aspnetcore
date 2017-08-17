import { SharepointListControlPage } from './app.po';

describe('sharepoint-list-control App', () => {
  let page: SharepointListControlPage;

  beforeEach(() => {
    page = new SharepointListControlPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!!');
  });
});

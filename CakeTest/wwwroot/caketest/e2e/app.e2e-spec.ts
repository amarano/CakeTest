import { CaketestPage } from './app.po';

describe('caketest App', function() {
  let page: CaketestPage;

  beforeEach(() => {
    page = new CaketestPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});

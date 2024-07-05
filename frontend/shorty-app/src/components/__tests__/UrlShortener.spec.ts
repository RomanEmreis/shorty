import { describe, it, expect, beforeEach } from 'vitest'
import { setActivePinia, createPinia } from 'pinia'
import { mount } from '@vue/test-utils'
import UrlShortener from '../UrlShortener.vue'
import { useUrlStore } from '@/stores/url';

const defaultExpectedHtml = `<div data-v-8174189b="" class="center-screen">
  <div data-v-f3006f2f="" data-v-8174189b="" class="url-input"><input data-v-f3006f2f="" type="text" placeholder="https://www.very-long-url.com" value=""><button data-v-f3006f2f="" class="generate-btn"><img data-v-f3006f2f="" alt="generate-img" src="/src/components/icons/generate.svg" width="38" height="38"></button></div>
  <!--v-if-->
  <!--v-if-->
  <div data-v-8174189b="" class="copyright">Created by Roman Emreis</div>
</div>`;

const withShortUrlExpectedHtml = `<div data-v-8174189b="" class="center-screen">
  <div data-v-f3006f2f="" data-v-8174189b="" class="url-input"><input data-v-f3006f2f="" type="text" placeholder="https://www.very-long-url.com" value=""><button data-v-f3006f2f="" class="generate-btn"><img data-v-f3006f2f="" alt="generate-img" src="/src/components/icons/generate.svg" width="38" height="38"></button></div>
  <div data-v-aadec475="" class="result-field"><a data-v-aadec475="" href="http://something" target="_blank">http://something</a><button data-v-aadec475="" class="copy-btn"><img data-v-aadec475="" alt="copy-img" src="/src/components/icons/copy.svg" width="38" height="38"></button></div>
  <!--v-if-->
  <div data-v-8174189b="" class="copyright">Created by Roman Emreis</div>
</div>`;

const withErrorExpectedHtml = `<div data-v-8174189b="" class="center-screen">
  <div data-v-f3006f2f="" data-v-8174189b="" class="url-input"><input data-v-f3006f2f="" type="text" placeholder="https://www.very-long-url.com" value=""><button data-v-f3006f2f="" class="generate-btn"><img data-v-f3006f2f="" alt="generate-img" src="/src/components/icons/generate.svg" width="38" height="38"></button></div>
  <!--v-if-->
  <div data-v-5427ef97="" class="error-field"><span data-v-5427ef97="">some error</span></div>
  <div data-v-8174189b="" class="copyright">Created by Roman Emreis</div>
</div>`;

describe('UrlShortener', () => {
  beforeEach(() => {
    setActivePinia(createPinia());
  });

  it('renders properly', () => {
    const wrapper = mount(UrlShortener, { props: {} });
    expect(wrapper.html()).toContain(defaultExpectedHtml);
  });

  it('with short URL renders properly', () => {
    const url = useUrlStore();
    url.new = 'http://something'

    const wrapper = mount(UrlShortener, { props: {} });
    expect(wrapper.html()).toContain(withShortUrlExpectedHtml);
  });

  it('with error renders properly', () => {
    const url = useUrlStore();
    url.error = 'some error'

    const wrapper = mount(UrlShortener, { props: {} });
    expect(wrapper.html()).toContain(withErrorExpectedHtml);
  });
});
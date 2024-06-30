import { describe, it, expect, beforeEach } from 'vitest'
import { setActivePinia, createPinia } from 'pinia'
import { mount } from '@vue/test-utils'
import UrlShortener from '../UrlShortener.vue'
import { useUrlStore } from '@/stores/url';

const defaultExpectedHtml = `<div data-v-8174189b="" class="center-screen">
  <div data-v-8174189b="" class="create-field"><input data-v-8174189b="" class="input-field" type="text" placeholder="https://www.very-long-url.com" value=""><button data-v-8174189b="" class="create-btn"><img data-v-8174189b="" alt="create-img" src="/src/components/icons/generate.svg" width="38" height="38"></button></div>
  <!--v-if-->
  <!--v-if-->
  <div data-v-8174189b="" class="copyright">Created by Roman Emreis &amp; AI</div>
</div>`;

const withShortUrlExpectedHtml = `<div data-v-8174189b="" class="center-screen">
  <div data-v-8174189b="" class="create-field"><input data-v-8174189b="" class="input-field" type="text" placeholder="https://www.very-long-url.com" value=""><button data-v-8174189b="" class="create-btn"><img data-v-8174189b="" alt="create-img" src="/src/components/icons/generate.svg" width="38" height="38"></button></div>
  <div data-v-8174189b="" class="result-field"><a data-v-8174189b="" class="result-text" href="http://something" target="_blank">http://something</a><button data-v-8174189b="" class="copy-btn"><img data-v-8174189b="" alt="copy-img" src="/src/components/icons/copy.svg" width="38" height="38"></button></div>
  <!--v-if-->
  <div data-v-8174189b="" class="copyright">Created by Roman Emreis &amp; AI</div>
</div>`;

const withErrorExpectedHtml = `<div data-v-8174189b="" class="center-screen">
  <div data-v-8174189b="" class="create-field"><input data-v-8174189b="" class="input-field" type="text" placeholder="https://www.very-long-url.com" value=""><button data-v-8174189b="" class="create-btn"><img data-v-8174189b="" alt="create-img" src="/src/components/icons/generate.svg" width="38" height="38"></button></div>
  <!--v-if-->
  <div data-v-8174189b="" class="error-field"><span data-v-8174189b="" class="error-text">some error</span></div>
  <div data-v-8174189b="" class="copyright">Created by Roman Emreis &amp; AI</div>
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
<template>
  <v-col
    cols="12"
    md="9"
  >
    <forum-list
      :category="category"
      :forums="data"
      :loading="meta.loading"
      class="mb-4"
    >
      <template #action>
        <div class="d-flex mb-2 mb-lg-3 flex-column flex-md-row">
          <v-text-field
            class="mb-2"
            v-debounce="(x) => query.search = x"
            :value="query.search"
            dense
            append-icon="search"
            label="Search"
            single-line
            hide-details
          >
          </v-text-field>
          <v-spacer class="d-none d-md-block"></v-spacer>
          <div class="d-flex">
            <auto-complete-select
              class="mr-3"
              label="Language"
              uri="languages/all"
              style="max-width: 300px"
              item-text="name"
              v-model="query.language"
              item-value="slug"
              dense
              single-line
              hide-details
            >
            </auto-complete-select>
            <v-spacer class="d-none d-md-block"></v-spacer>
            <forum-sort-select
              class="mb-2"
              style="max-width: 200px"
              v-model="query.sort"
              dense
              single-line
              hide-details
            >
            </forum-sort-select>
          </div>
        </div>
      </template>

      <template #footer>
        <v-pagination
          v-model="query.page"
          :total-visible="$vuetify.breakpoint.mdAndUp ? 8 : 4"
          :length="meta.totalPages"
        ></v-pagination>
      </template>
    </forum-list>
  </v-col>
</template>

<script lang="ts">
import { defineComponent, reactive, watch } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import ForumList from '@/components/forum/ForumList.vue';
import { useCategories } from '@/composable/form';
import { useRoute, useRouter } from '@/composable/compat';
import { useQuery } from '@/services/http';
import { Category, Dictionary } from '@/services/model';
import { useBreadcrumbs } from '@/composable/breadcrumbs';
import ForumSortSelect from '@/components/form/ForumSortSelect.vue';
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';

export default defineComponent({
  name: 'ForumIndex',
  components: { AutoCompleteSelect, ForumSortSelect, ForumList },
  async beforeRouteEnter(to, from, next) {
    if (!to.query.category) return next({ name: 'NotFound' });
    const category = Array.isArray(to.query.category) ? to.query.category[0] : to.query.category;
    const { data, fetch, loading } = useCategories();
    if (loading.value) await fetch();
    if (!data.value.some((x) => x.slug === category)) return next({ name: 'NotFound' });
    return next();
  },
  setup() {
    const route = useRoute();
    const router = useRouter();
    const category = reactive({ ...(useCategories().data.value.find((x) => x.slug === route.query.category) as Category) });

    useTitle(`${category.name} Forums`);
    useBreadcrumbs([
      { text: 'Home', name: 'Home' },
      { text: 'Forums', name: 'Search' },
      { text: category.name, disabled: true },
    ]);

    watch(() => route.query.category, (val) => {
      const newCategory = useCategories().data.value.find((x) => x.slug === val);
      if (!newCategory) {
        router.push({ name: 'NotFound' });
        return;
      }
      Object.assign(category, newCategory);
      query.search = '';
      useTitle(`${category.name} Forums`);
      useBreadcrumbs([
        { text: 'Home', name: 'Home' },
        { text: 'Forums', name: 'Search' },
        { text: category.name, disabled: true },
      ]);
    });

    const { query, data, meta } = useQuery<Dictionary>('forums')({
      category: '',
      search: '',
      language: '',
      sort: '',
    });

    return {
      query,
      data,
      category,
      meta,
    };
  },
});
</script>

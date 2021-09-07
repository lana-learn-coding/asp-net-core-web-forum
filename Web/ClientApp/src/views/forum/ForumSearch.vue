<template>
  <v-col cols="12" md="9">
    <v-card flat>
      <v-card-title>Forums {{ query.search ? 'Search Result' : 'List' }}</v-card-title>
      <v-card-subtitle v-if="query.search">Keyword "{{ query.search }}"</v-card-subtitle>
      <v-card-subtitle v-else>You can search using search bar</v-card-subtitle>

      <v-card-text class="pt-0">
        <div class="d-flex mb-2 mb-lg-3 flex-column flex-md-row">
          <category-select
            style="max-width: 300px"
            v-model="query.category"
            dense
            item-value="slug"
            single-line
            hide-details
          >
          </category-select>
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
        <v-skeleton-loader v-if="meta.loading" type="list-item-avatar-three-line@4">
        </v-skeleton-loader>
        <div v-else-if="!data.length">
          <v-divider></v-divider>
          <div class="body-1 text-center pa-8">Nothing here :(</div>
        </div>
        <div v-else v-for="forum in data" :key="forum.uid">
          <v-divider></v-divider>
          <forum-row :forum="forum"></forum-row>
        </div>
        <v-pagination
          v-model="query.page"
          :total-visible="$vuetify.breakpoint.mdAndUp ? 8 : 4"
          :length="meta.totalPages"
        ></v-pagination>
      </v-card-text>
    </v-card>
  </v-col>
</template>

<script lang="ts">
import { defineComponent, watch } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import ForumList from '@/components/forum/ForumList.vue';
import { useQuery } from '@/services/http';
import { Dictionary } from '@/services/model';
import { useBreadcrumbs } from '@/composable/breadcrumbs';
import ForumRow from '@/components/forum/ForumRow.vue';
import { useRoute } from '@/composable/compat';
import ForumSortSelect from '@/components/form/ForumSortSelect.vue';
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';
import CategorySelect from '@/components/form/CategorySelect.vue';

export default defineComponent({
  name: 'ForumSearch',
  components: { CategorySelect, AutoCompleteSelect, ForumSortSelect, ForumList, ForumRow },
  setup() {
    useTitle('Forums Search');
    useBreadcrumbs([
      { text: 'Home', name: 'Home' },
      { text: 'Forums', name: 'Home' },
      { text: 'Search', disabled: true },
    ]);

    const { query, data, meta } = useQuery<Dictionary>('forums')({
      category: '',
      search: '',
      language: '',
      sort: '',
    });

    const route = useRoute();
    watch(() => route.query.search, (val) => {
      if (query.search !== 'val') {
        query.search = val as unknown as string;
      }
    });

    return {
      query,
      data,
      meta,
    };
  },
});
</script>

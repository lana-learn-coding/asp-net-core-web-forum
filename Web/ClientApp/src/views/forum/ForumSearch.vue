<template>
  <v-row>
    <v-col cols="12" md="9">
      <v-card flat>
        <v-card-title>Forums Search Result</v-card-title>
        <v-card-subtitle>Keyword "{{ query.search }}"</v-card-subtitle>

        <v-card-text class="pt-0">
          <forum-sort-select
            class="mb-3"
            style="max-width: 200px"
            v-model="query.sort"
            dense
            single-line
            hide-details
          >
          </forum-sort-select>
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

    <v-col cols="12" md="3">
      <app-forum-statistics></app-forum-statistics>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { defineComponent, watch } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import ForumList from '@/components/forum/ForumList.vue';
import AppForumStatistics from '@/components/app/AppForumStatistics.vue';
import { useQuery } from '@/services/http';
import { Dictionary } from '@/services/model';
import { useBreadcrumbs } from '@/composable/breadcrumbs';
import ForumRow from '@/components/forum/ForumRow.vue';
import { useRoute } from '@/composable/compat';
import ForumSortSelect from '@/components/form/ForumSortSelect.vue';

export default defineComponent({
  name: 'ForumSearch',
  components: { ForumSortSelect, ForumList, AppForumStatistics, ForumRow },
  setup() {
    useTitle('Forums Search');
    useBreadcrumbs([
      { text: 'Home', name: 'Home' },
      { text: 'Forums', name: 'Home' },
      { text: 'Search', disabled: true },
    ]);

    const { query, data, meta } = useQuery<Dictionary>('forums')({
      category: '_all',
      search: '',
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

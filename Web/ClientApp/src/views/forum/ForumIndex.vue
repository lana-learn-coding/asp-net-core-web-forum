<template>
  <v-row>
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
          <div class="d-flex mb-3 mb-lg-5">
            <v-text-field
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

    <v-col
      cols="12"
      md="3"
    >
      <app-forum-statistics></app-forum-statistics>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { defineComponent, reactive, watch } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import ForumList from '@/components/forum/ForumList.vue';
import AppForumStatistics from '@/components/app/AppForumStatistics.vue';
import { useCategories } from '@/composable/form';
import { useRoute, useRouter } from '@/composable/compat';
import { useQuery } from '@/services/http';
import { Dictionary } from '@/services/model';
import { useBreadcrumbs } from '@/composable/breadcrumbs';

export default defineComponent({
  name: 'ForumIndex',
  components: { ForumList, AppForumStatistics },
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
    const category = reactive({ ...useCategories().data.value.find((x) => x.slug === route.query.category) });

    useTitle(`${category.name} Forums`);
    useBreadcrumbs([
      { text: 'Home', name: 'Home' },
      { text: 'Forums', name: 'Home' },
      { text: category.name, disabled: true },
    ]);

    watch(() => route.query.category, (val) => {
      const newCategory = useCategories().data.value.find((x) => x.slug === val);
      if (!newCategory) {
        router.push({ name: 'NotFound' });
        return;
      }
      Object.assign(category, newCategory);
      useTitle(`${category.name} Forums`);
      useBreadcrumbs([
        { text: 'Home', name: 'Home' },
        { text: 'Forums', name: 'Home' },
        { text: category.name, disabled: true },
      ]);
    });

    const { query, data, meta } = useQuery<Dictionary>('forums')({
      category: '',
      search: '',
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

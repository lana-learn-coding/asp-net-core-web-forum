<template>
  <div id="app">
    <v-navigation-drawer
      v-model="drawer"
      app
    >
      <v-list-item>
        <v-list-item-content>
          <v-list-item-title class="title">
            <app-icon></app-icon>
          </v-list-item-title>
          <v-list-item-subtitle>
            Doctors Forums
          </v-list-item-subtitle>
        </v-list-item-content>
      </v-list-item>

      <v-divider></v-divider>

      <v-list
        dense
        nav
      >
        <v-list-item-group color="primary">
          <template v-for="item in links">

            <v-list-group
              v-if="item.links"
              :key="item.title"
              :prepend-icon="item.icon"
            >
              <template v-slot:activator>
                <v-list-item-title>Topic</v-list-item-title>
              </template>
              <v-list-item
                v-for="link in item.links"
                :key="link.title"
                :to="link.name ? { name: link.name } : null"
                exact-path
                link
              >
                <v-list-item-icon>
                  <v-icon>{{ link.icon || 'dashboard' }}</v-icon>
                </v-list-item-icon>

                <v-list-item-content>
                  <v-list-item-title>{{ link.title }}</v-list-item-title>
                </v-list-item-content>
              </v-list-item>
            </v-list-group>

            <v-list-item
              v-else
              :key="item.title"
              :to="item.name ? { name: item.name } : null"
              exact-path
              link
            >
              <v-list-item-icon>
                <v-icon>{{ item.icon || 'dashboard' }}</v-icon>
              </v-list-item-icon>

              <v-list-item-content>
                <v-list-item-title>{{ item.title }}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </template>
        </v-list-item-group>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar app color="primary" dark>
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>

      <v-toolbar-title>{{ title }}</v-toolbar-title>
    </v-app-bar>

    <v-main>
      <v-container class="mt-3 mt-md-4 mt-lg-5 mt-xl-6">
        <router-view></router-view>
      </v-container>
    </v-main>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import { useVuetify } from '@/composable/compat';
import AppIcon from '@/components/app/AppIcon.vue';

export default defineComponent({
  name: 'Admin',
  components: { AppIcon },
  setup() {
    const title = useTitle('Admin', { observe: true });
    const drawer = ref(useVuetify().breakpoint.lgAndUp);

    const links = [
      { title: 'Dashboard', icon: 'bar_chart', name: 'Dashboard' },
      {
        title: 'Topics',
        icon: 'style',
        links: [
          { title: 'Category', icon: 'label_important', name: 'ManageCategory' },
          { title: 'Tag', icon: 'local_offer', name: 'ManageTag' },
        ],
      },
      { title: 'Forum', icon: 'forum', name: 'ManageForum' },
      { title: 'Users', icon: 'person', name: 'ManageUser' },
      { title: 'Home', icon: 'home', name: 'Home' },
    ];

    return {
      title,
      drawer,
      links,
    };
  },
});
</script>
